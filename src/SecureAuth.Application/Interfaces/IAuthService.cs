using SecureAuth.Application.DTOs.User;
using SecureAuth.Application.DTOs.RefreshToken;
using SecureAuth.Application.Common;

namespace SecureAuth.Application.Interfaces 
{
    public interface IAuthService 
    {
        Task<Result<UserSignUpResponseDto>> SignUpUser (UserSignUpRequestDto userRegistrationRequestDto, string userEmailFromJwt);
        Task<Result<UserSignInResponseDto>> SignInUser(UserSignInRequestDto userLoginRequestDto);
        Task<Result<RefreshTokenResponseDto>> TokenRefresh(RefreshTokenRequestDto refreshTokenRequestDto);
        Task<Result<UserSignOutResponseDto>> SignOutUser(UserSignOutRequestDto userSignOutRequestDto);
    }
}