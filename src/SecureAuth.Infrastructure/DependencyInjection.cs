using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SecureAuth.Application.Abstractions;
using SecureAuth.Infrastructure.Security.JwtTokenProvider;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using System.Text;
using SecureAuth.Infrastructure.Security.PasswordHasher;
using SecureAuth.Infrastructure.Email;
using SecureAuth.Infrastructure.Presistence.Repositories;
using SecureAuth.Infrastructure.Persistence.ApplicationDbContext;
using SecureAuth.Infrastructure.Presistence;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Channels;
using SecureAuth.Application.Jobs;
using SecureAuth.Infrastructure.Channels;
using SecureAuth.Infrastructure.BackgroundServices;
using SecureAuth.Infrastructure.Security;

namespace SecureAuth.Infrastructure 
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<JwtOptions>().BindConfiguration(nameof(JwtOptions))
                     .ValidateDataAnnotations()
                     .ValidateOnStart();
            services.AddOptions<PasswordOptions>().BindConfiguration(nameof(PasswordOptions))
                     .ValidateDataAnnotations()
                     .ValidateOnStart();
            services.AddOptions<SmtpOptions>().BindConfiguration(nameof(SmtpOptions))
                     .ValidateDataAnnotations()
                     .ValidateOnStart();
            services.AddOptions<RefreshTokenOptions>().BindConfiguration(nameof(RefreshTokenOptions))
                     .ValidateDataAnnotations()
                     .ValidateOnStart();

            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName))
                .UseSnakeCaseNamingConvention());


            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEmailVerificationRepository, EmailVerificationRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddSingleton<ITokenProvider,JwtTokenProvider>();
            services.AddSingleton<IPasswordHasher, PBKDF2>();
            services.AddSingleton<IEmailSender, SmtpEmailSender>();
            services.AddSingleton(Channel.CreateUnbounded<EmailJob>());
            services.AddSingleton<EmailJobQueue>();
            services.AddSingleton<IEmailJobQueue>(sp => sp.GetRequiredService<EmailJobQueue>());
            services.AddHostedService<EmailBackgroundService>();
            services.AddSingleton(TimeProvider.System);

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); 
            
            services.AddAuthentication()
                    .AddJwtBearer(AppAuthenticationScheme.Access)
                    .AddJwtBearer(AppAuthenticationScheme.Registration);

            services.AddOptions<JwtBearerOptions>(AppAuthenticationScheme.Access).Configure<IOptions<JwtOptions>>((options, jwtOptions)=>
            {
                var jwt = jwtOptions.Value;
                
                options.MapInboundClaims = false;

                options.TokenValidationParameters = new TokenValidationParameters 
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,

                    ValidIssuer = jwt.Issuer,
                    ValidAudience = jwt.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwt.AccessKey)
                    ),
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddOptions<JwtBearerOptions>(AppAuthenticationScheme.Registration).Configure<IOptions<JwtOptions>>((options, jwtOptions) => 
            {
                var jwt = jwtOptions.Value;

                options.MapInboundClaims = false;


                options.TokenValidationParameters = new TokenValidationParameters 
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,

                    ValidIssuer = jwt.Issuer,
                    ValidAudience = jwt.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwt.RegistrationKey)
                    ),

                    ClockSkew = TimeSpan.Zero
                };
            });
            
            services.AddAuthorization(options =>
            {
                options.AddPolicy(AppAuthorizationPolicy.Registration, policy =>
                {
                    policy.AuthenticationSchemes.Add(AppAuthenticationScheme.Registration); 
                    policy.RequireClaim("email"); 
                    policy.RequireAuthenticatedUser();
                });

                options.AddPolicy(AppAuthorizationPolicy.Access, policy =>
                {
                    policy.AuthenticationSchemes.Add(AppAuthenticationScheme.Access);
                    policy.RequireAuthenticatedUser();
                } );
            });

            return services;
        }
    }
}