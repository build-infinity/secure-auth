using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecureAuth.Domain.Models;

namespace SecureAuth.Infrastructure.Presistence.Configurations
{
    internal sealed class EmailVerificationConfiguration : IEntityTypeConfiguration<EmailVerification>
    {
        public void Configure(EntityTypeBuilder<EmailVerification> builder)
        {
            builder.ToTable("email_verifications");
            builder.HasKey(ev => ev.VerificationId);

            builder.ToTable( t => {
                t.HasCheckConstraint("ck_email_verifications_attempt_count", "attempt_count between 0 and 5");
            });
                
        }
    }
}