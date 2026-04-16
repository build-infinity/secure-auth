using SecureAuth.Application.DTOs.User;
using SecureAuth.Application.DTOs.RefreshToken;
using SecureAuth.Application.Common;

namespace SecureAuth.Application.Interfaces 
{
    public interface IAuthService 
    {
        Task<Result<UserSignUpResponseDto>> SignUpUser (UserSignUpRequestDto userRegistrationRequestDto, string userEmailFromJwt, CancellationToken cancellationToken);
        Task<Result<UserSignInResponseDto>> SignInUser(UserSignInRequestDto userLoginRequestDto, CancellationToken cancellationToken);
        Task<Result<RefreshTokenResponseDto>> TokenRefresh(RefreshTokenRequestDto refreshTokenRequestDto, CancellationToken cancellationToken);
        Task<Result<UserSignOutResponseDto>> SignOutUser(UserSignOutRequestDto userSignOutRequestDto, CancellationToken cancellationToken);
    }
}