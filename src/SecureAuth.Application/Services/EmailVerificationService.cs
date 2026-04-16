using SecureAuth.Application.Abstractions;
using SecureAuth.Application.Interfaces;
using SecureAuth.Application.Common;
using SecureAuth.Application.DTOs.EmailVerification;
using SecureAuth.Application.Errors;
using SecureAuth.Domain.Models;
using System.Security.Cryptography;
using SecureAuth.Application.Consts;

namespace SecureAuth.Application.Services
{
    internal sealed class EmailVerificationService : IEmailVerificationService
    {
        private readonly IEmailVerificationRepository _emailVerificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenProvider _tokenProvider;

        public EmailVerificationService 
        (
            IUserRepository userRepository, 
            IEmailVerificationRepository emailVerificationRepository, 
            IEmailService emailService,
            IUnitOfWork unitOfWork,
            ITokenProvider tokenProvider
        )
        {
            _userRepository = userRepository;
            _emailVerificationRepository = emailVerificationRepository;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
            _tokenProvider = tokenProvider;
        }

        public async Task<Result<EmailOtpResponseDto>> SendEmailVerificationOtp(EmailOtpRequestDto emailOtpRequestDto, CancellationToken cancellationToken)
        {
            if(await _userRepository.EmailExistsAsync(emailOtpRequestDto.Email, cancellationToken))
                 return EmailErrors.AlreadyExists;

            var emailVerificationExists= await _emailVerificationRepository.GetVerificationByEmailAsync(emailOtpRequestDto.Email, cancellationToken);
            
           if( emailVerificationExists is not null && emailVerificationExists.OtpExpiresOnUtc > DateTime.UtcNow)                
           {
               TimeSpan remaining = emailVerificationExists.OtpExpiresOnUtc - DateTime.UtcNow;
                   return OtpErrors.AlreadySent((int)Math.Ceiling(remaining.TotalSeconds));
           }

           var otp = GenerateOTP();

            EmailVerification emailVerification = new EmailVerification 
            {
                VerificationId = Guid.NewGuid(),
                Email = emailOtpRequestDto.Email,
                OtpHash = Helper.Hash(otp),
                OtpExpiresOnUtc = DateTime.UtcNow.AddMinutes(Otp.ExpiresInMinutes),
                CreatedOnUtc = DateTime.UtcNow
            };

            _emailVerificationRepository.Add(emailVerification);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _emailService.SendOtp(otp, emailOtpRequestDto.Email, cancellationToken);
            
            return new EmailOtpResponseDto() { 
                VerificationId = emailVerification.VerificationId, OtpExpiresAtUtc = emailVerification.OtpExpiresOnUtc
            };
        }

        public async Task<Result<VerifyEmailResponseDto>> VerifyEmail(VerifyEmailRequestDto emailRequestDto, CancellationToken cancellationToken)
        {
            var emailVerification = await _emailVerificationRepository.GetByIdAsync(emailRequestDto.VerificationId, cancellationToken);

           var error = OtpValidator.Validate(emailVerification);

           if(error is not null) 
               return error;

           if(!Helper.Verify(data : emailRequestDto.Otp, storedDataHash : emailVerification!.OtpHash))
            {
                emailVerification.AttemptCount++;
                await _unitOfWork.SaveChangesAsync();
                return OtpErrors.Invalid;
            }

           emailVerification.IsUsed = true;

           var tokenResult = _tokenProvider.GenerateRegistrationToken(emailVerification.Email); 

           await _unitOfWork.SaveChangesAsync(cancellationToken);

           return new VerifyEmailResponseDto() { 
               RegistrationToken = tokenResult.Token,  ExpiresOnUtc = tokenResult.ExpiresOnUtc
            };
        }
        private string GenerateOTP(int length = 6)
        {
            char[] chars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};

            char[] result = new char[length];

            for(int i = 0; i < length; i++)
            {
                int rn = RandomNumberGenerator.GetInt32(0,chars.Length);
                result[i] = chars[rn];
            }

            return new string(result);
        }
    }
}