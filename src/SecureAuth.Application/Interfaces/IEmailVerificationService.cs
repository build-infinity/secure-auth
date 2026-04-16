using SecureAuth.Application.Common;
using SecureAuth.Application.DTOs.EmailVerification;

namespace SecureAuth.Application.Interfaces
{
    public interface IEmailVerificationService
    {
        Task<Result<EmailOtpResponseDto>> SendEmailVerificationOtp(EmailOtpRequestDto emailOtpRequestDto, CancellationToken cancellationToken);
        Task<Result<VerifyEmailResponseDto>> VerifyEmail(VerifyEmailRequestDto emailRequestDto, CancellationToken cancellationToken);
    }
}