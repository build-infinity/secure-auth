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
        public async Task<IActionResult> SendOtp([FromBody] EmailOtpRequestDto emailOtpRequestDto)
        {
            var result = await _emailVerificationService.SendEmailVerificationOtp(emailOtpRequestDto);

            return result.ToActionResult();
        }

        [HttpPost("email-verification/verify")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequestDto verifyEmailRequestDto)
        {
            var result = await _emailVerificationService.VerifyEmail(verifyEmailRequestDto);

            return result.ToActionResult();
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] UserSignInRequestDto userSignInRequestDto)
        {
            var result = await _authService.SignInUser(userSignInRequestDto);

            return result.ToActionResult();
        }

        [Authorize(Policy = AppAuthorizationPolicy.Registration)]
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody]UserSignUpRequestDto userSignUpRequestDto)
        {
            var email = User.FindFirst(JwtRegisteredClaimNames.Email)!.Value;

            var result = await _authService.SignUpUser(userSignUpRequestDto, email);

            return result.ToActionResult();
        }

        [Authorize(Policy = AppAuthorizationPolicy.Access)]
        [HttpPost("signout")]
        public async Task<IActionResult> SignOut([FromBody]UserSignOutRequestDto userSignOutRequestDto)
        {
            var result = await _authService.SignOutUser(userSignOutRequestDto);

            return result.ToActionResult();
        }
        
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken ([FromBody] RefreshTokenRequestDto refreshTokenRequestDto)
        {
            var result = await _authService.TokenRefresh(refreshTokenRequestDto);

            return result.ToActionResult();
        }
    }
}