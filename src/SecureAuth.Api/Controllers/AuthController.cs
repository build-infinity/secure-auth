using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureAuth.Application.DTOs.RefreshToken;
using SecureAuth.Application.DTOs.EmailVerification;
using SecureAuth.Application.DTOs.User;
using SecureAuth.Application.Interfaces;
using SecureAuth.Infrastructure.Security;

namespace SecureAuth.Api.Controllers 
{
    [ApiController] 
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmailVerificationService _emailVerificationService;

        public AuthController(IAuthService authService, IEmailVerificationService emailVerificationService)
        {
            _authService = authService;
            _emailVerificationService = emailVerificationService;
        }

        [HttpPost("email-verification/otp")]
        public async Task<IActionResult> SendOtp([FromBody] EmailOtpRequestDto emailOtpRequestDto, CancellationToken cancellationToken)
        {
            var result = await _emailVerificationService.SendEmailVerificationOtp(emailOtpRequestDto, cancellationToken);

            return result.ToActionResult();
        }

        [HttpPost("email-verification/verify")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequestDto verifyEmailRequestDto, CancellationToken cancellationToken)
        {
            var result = await _emailVerificationService.VerifyEmail(verifyEmailRequestDto, cancellationToken);

            return result.ToActionResult();
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] UserSignInRequestDto userSignInRequestDto, CancellationToken cancellationToken)
        {
            var result = await _authService.SignInUser(userSignInRequestDto, cancellationToken);

            return result.ToActionResult();
        }

        [Authorize(Policy = AppAuthorizationPolicy.Registration)]
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody]UserSignUpRequestDto userSignUpRequestDto, CancellationToken cancellationToken)
        {
            var email = User.FindFirst(JwtRegisteredClaimNames.Email)!.Value;

            var result = await _authService.SignUpUser(userSignUpRequestDto, email, cancellationToken);

            return result.ToActionResult();
        }

        [Authorize(Policy = AppAuthorizationPolicy.Access)]
        [HttpPost("signout")]
        public async Task<IActionResult> SignOut([FromBody]UserSignOutRequestDto userSignOutRequestDto, CancellationToken cancellationToken)
        {
            var result = await _authService.SignOutUser(userSignOutRequestDto, cancellationToken);

            return result.ToActionResult();
        }
        
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken ([FromBody] RefreshTokenRequestDto refreshTokenRequestDto, CancellationToken cancellationToken)
        {
            var result = await _authService.TokenRefresh(refreshTokenRequestDto, cancellationToken);

            return result.ToActionResult();
        }
    }
}