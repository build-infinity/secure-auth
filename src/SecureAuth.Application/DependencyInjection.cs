using Microsoft.Extensions.DependencyInjection;
using SecureAuth.Application.Interfaces;
using SecureAuth.Application.Services;

namespace SecureAuth.Application 
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IAuthService,AuthService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IEmailVerificationService, EmailVerificationService>();

            return services;
        }
    }
}