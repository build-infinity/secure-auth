using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SecureAuth.Application.Common;
using SecureAuth.Application.Abstractions;
using SecureAuth.Domain.Models;

namespace SecureAuth.Infrastructure.Security.JwtTokenProvider
{
    internal sealed class JwtTokenProvider : ITokenProvider
    {
        private readonly TimeProvider _timeProvider;
        private readonly JwtOptions _jwtOptions;
        private readonly RefreshTokenOptions _refreshTokenOptions;

        public JwtTokenProvider(IOptions<JwtOptions> jwtOptions,TimeProvider timeProvider,IOptions<RefreshTokenOptions> refreshTokenOptions)
        {
            _jwtOptions = jwtOptions.Value;
            _timeProvider = timeProvider;
            _refreshTokenOptions = refreshTokenOptions.Value;
        }
        public TokenResult GenerateAccessToken(User user)
        {
            Claim[] claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.UserId.ToString()),
                new Claim (JwtRegisteredClaimNames.GivenName,user.FirstName),
                new Claim(JwtRegisteredClaimNames.FamilyName,user.LastName),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            var expiresOnUtc = _timeProvider.GetUtcNow().AddMinutes(_jwtOptions.AccessExpiresInMinutes).UtcDateTime;
            string token = GenerateToken(
                key : _jwtOptions.AccessKey,
                claims : claims,
                expiresAt : expiresOnUtc
            );

            return new TokenResult{ Token = token, ExpiresOnUtc = expiresOnUtc }; 
        }
        public TokenResult GenerateRegistrationToken(string email)
        {
            Claim[] claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Email,email)
            };

            var expiresOnUtc = _timeProvider.GetUtcNow().AddMinutes(_jwtOptions.RegistrationExpiresInMinutes).UtcDateTime;
            
            var token =  GenerateToken (
                key : _jwtOptions.RegistrationKey,
                claims : claims,
                expiresAt : expiresOnUtc
            );

            return new TokenResult { Token = token, ExpiresOnUtc = expiresOnUtc };
        }

        public TokenResult GenerateRefreshToken()
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(_refreshTokenOptions.SizeInBytes));

            return new TokenResult { 
                Token = token, 
                ExpiresOnUtc = _timeProvider.GetUtcNow().AddDays(_refreshTokenOptions.LifeTimeDays).UtcDateTime
             };
        }
        private string GenerateToken(string key, Claim[] claims, DateTime expiresAt)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            // HMAC - Hash-based Message Authentication Code
            string algorithm  = _jwtOptions.HmacAlgorithm switch 
            {
                HmacAlgorithm.HS256 => SecurityAlgorithms.HmacSha256,
                HmacAlgorithm.HS512 => SecurityAlgorithms.HmacSha512,

                 _ => throw new InvalidOperationException("unsupported jwt algorithm")
            };

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(keyBytes);

            SigningCredentials credentials = new SigningCredentials(securityKey, algorithm);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer : _jwtOptions.Issuer,
                audience : _jwtOptions.Audience,
                claims : claims,
                expires : expiresAt,
                signingCredentials : credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
} 