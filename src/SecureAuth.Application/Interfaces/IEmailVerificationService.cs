using SecureAuth.Application.Common;
using SecureAuth.Application.DTOs.EmailVerification;

namespace SecureAuth.Application.Interfaces
{
    public interface IEmailVerificationService
    {
        Task<Result<EmailOtpResponseDto>> SendEmailVerificationOtp(EmailOtpRequestDto emailOtpRequestDto);
        Task<Result<VerifyEmailResponseDto>> VerifyEmail(VerifyEmailRequestDto emailRequestDto);
    }
}