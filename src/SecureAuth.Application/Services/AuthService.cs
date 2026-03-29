using SecureAuth.Application.Abstractions;
using SecureAuth.Application.DTOs.User;
using SecureAuth.Domain.Models;
using SecureAuth.Application.Interfaces;
using SecureAuth.Application.DTOs.RefreshToken;
using SecureAuth.Application.Common;
using SecureAuth.Application.Errors;
using SecureAuth.Application.Validators;
using SecureAuth.Api;

namespace SecureAuth.Application.Services 
{
    internal sealed class AuthService : IAuthService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenProvider _tokenProvider;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AuthService(
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IRoleRepository roleRepository,
            IUnitOfWork unitOfWork,
            ITokenProvider tokenProvider,
            IPasswordHasher passwordHasher
        )   
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
            _tokenProvider = tokenProvider;
            _passwordHasher = passwordHasher;
        }
       
        public async Task<Result<UserSignUpResponseDto>> SignUpUser (UserSignUpRequestDto userRegistrationRequestDto, string userEmailFromJwt)
        {    
            if(await _userRepository.EmailExistsAsync(userEmailFromJwt))
                 return EmailErrors.AlreadyExists;

            var error = PasswordValidator.Validate(userRegistrationRequestDto.Password.Trim());

            if(error is not null)
               return error;

            var defaultRole = await _roleRepository.GetByName(Roles.User) ?? throw new Exception(); //must be custom excetion

            User user = new User 
            {
                UserId = Guid.NewGuid(),
                FirstName = userRegistrationRequestDto.FirstName,
                LastName = userRegistrationRequestDto.LastName,
                PasswordHash = _passwordHasher.Hash(userRegistrationRequestDto.Password),
                Email = userEmailFromJwt,
                NormalizedEmail = userEmailFromJwt.ToUpper(),
                CreatedOnUtc = DateTime.UtcNow
            };

            user.UserRoles.Add(new UserRole { UserId = user.UserId, RoleId = defaultRole.RoleId, CreatedOnOtc = DateTime.UtcNow } );

             var refreshTokenResult = _tokenProvider.GenerateRefreshToken();
             RefreshToken token = new RefreshToken()
             {
                 TokenId = Guid.NewGuid(),
                 TokenHash = Helper.Hash(refreshTokenResult.Token),
                 ExpiresOnUtc = refreshTokenResult.ExpiresOnUtc,
                 UserId = user.UserId,
                 CreatedOnUtc = DateTime.UtcNow
             };

            var accessTokenResult = _tokenProvider.GenerateAccessToken(user);

            _userRepository.Add(user);
            _refreshTokenRepository.Add(token);
            await _unitOfWork.SaveChangesAsync();

            return new UserSignUpResponseDto() { 
                AccessToken = accessTokenResult.Token,
                AccessTokenExpiresOnUtc = accessTokenResult.ExpiresOnUtc, 
                RefreshToken = refreshTokenResult.Token, 
                RefreshTokenExpiresOnUtc = refreshTokenResult.ExpiresOnUtc 
            };
        }

        public async Task<Result<UserSignInResponseDto>> SignInUser(UserSignInRequestDto userSignInRequestDto)
        {
            var verifiedUser = await _userRepository.GetByNormalizedEmailAsync(userSignInRequestDto.Email.ToUpperInvariant());
            if(verifiedUser is null)
                return AuthErrors.InvalidCredentials;
            
            if(!_passwordHasher.Verify(userSignInRequestDto.Password, verifiedUser.PasswordHash))
                 return AuthErrors.InvalidCredentials;

            var accessTokenResult = _tokenProvider.GenerateAccessToken(verifiedUser);

            var refreshTokenResult = _tokenProvider.GenerateRefreshToken();

            RefreshToken refreshToken = new RefreshToken()
            {
                TokenId = Guid.NewGuid(),
                TokenHash = Helper.Hash(refreshTokenResult.Token),
                ExpiresOnUtc = refreshTokenResult.ExpiresOnUtc,
                UserId = verifiedUser.UserId,
                CreatedOnUtc = DateTime.UtcNow
            };

            _refreshTokenRepository.Add(refreshToken);
            await _unitOfWork.SaveChangesAsync();

            return new UserSignInResponseDto(){ 
                AccessToken = accessTokenResult.Token, AccessTokenExpiresOnUtc = accessTokenResult.ExpiresOnUtc, 
                RefreshToken = refreshTokenResult.Token, RefreshTokenExpiresOnUtc = refreshTokenResult.ExpiresOnUtc
            };
        }

        public async Task<Result<UserSignOutResponseDto>> SignOutUser(UserSignOutRequestDto userSignOutRequestDto)
        {
            var tokenHash = Helper.Hash(userSignOutRequestDto.RefreshToken);

            var storedRefreshToken = await _refreshTokenRepository.GetByTokenHashAsync(tokenHash);

            if(storedRefreshToken is null) 
                return AuthErrors.RefreshTokenInvalid;

             var error = RefreshTokenValidator.Validate(storedRefreshToken);

             if(error is not null)
                return error;
            
            if(!await _refreshTokenRepository.TryRevokeAsync(storedRefreshToken.TokenHash))
               return AuthErrors.RefreshTokenAlreadyRevoked;

            return new UserSignOutResponseDto { Message = "Sign out successfully"};
        }

        public async Task<Result<RefreshTokenResponseDto>> TokenRefresh(RefreshTokenRequestDto refreshTokenRequestDto)
        {
            var tokenHash = Helper.Hash(refreshTokenRequestDto.RefreshToken);
    
            var storedToken = await _refreshTokenRepository.GetByTokenHashAsync(tokenHash);

            if(storedToken is null)
                return AuthErrors.RefreshTokenInvalid;

            var error = RefreshTokenValidator.Validate(storedToken);

            if(error is not null) 
                 return error;

            if(!await _refreshTokenRepository.TryRevokeAsync(storedToken.TokenHash))
                return AuthErrors.RefreshTokenAlreadyRevoked; 

            var refreshTokenResult = _tokenProvider.GenerateRefreshToken();

            RefreshToken token = new RefreshToken()
            {
                TokenId = Guid.NewGuid(),
                TokenHash = Helper.Hash(refreshTokenResult.Token),
                ExpiresOnUtc = refreshTokenResult.ExpiresOnUtc,
                UserId = storedToken.UserId,
                CreatedOnUtc = DateTime.UtcNow
            };

            var accessTokenResult = _tokenProvider.GenerateAccessToken(storedToken.User);

            _refreshTokenRepository.Add(token);
            await _unitOfWork.SaveChangesAsync();

            return new RefreshTokenResponseDto{ 
                AccessToken = accessTokenResult.Token, 
                AccessTokenExpiresOnUtc = accessTokenResult.ExpiresOnUtc,
                RefreshToken = refreshTokenResult.Token,
                RefreshTokenExpiresOnUtc = refreshTokenResult.ExpiresOnUtc
            };           
        }
    }
}