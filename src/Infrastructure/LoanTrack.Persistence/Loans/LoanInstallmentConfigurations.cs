using LoanTrack.Domain.Loans;
using LoanTrack.Persistence.Common.Database.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanTrack.Persistence.Loans;

internal class LoanInstallmentConfigurations : BaseEntityConfiguration<LoanInstallment>
{
    public override void Configure(EntityTypeBuilder<LoanInstallment> builder)
    {
        builder.ToTable("LoanInstallments");
        
        builder.HasOne<Loan>(x=>x.Loan)
            .WithMany()
            .HasForeignKey(x=>x.LoanId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(x=>x.InstallmentNumber)
            .IsRequired();
        
        builder.Property(x=>x.InstallmentAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.InstallmentDate)
            .IsRequired();

        builder.Property(x => x.IsPaid)
            .HasDefaultValue(false);
        
        builder.Property(x=>x.IsDelayed)
            .HasDefaultValue(false);
        
        builder.Property(x=>x.DelayedDays)
            .HasDefaultValue(0);
        
        builder.Property(x=>x.IsPenaltyApplied)
            .HasDefaultValue(false);

        builder.Property(x => x.PenaltyAmount)
            .HasPrecision(18, 2)
            .HasDefaultValue(0.00);
        
        builder.Property(x=>x.PenaltyReason)
            .HasMaxLength(300)
            .IsRequired(false);
        
        builder.Property(x=>x.PaymentMethod)
            .HasMaxLength(30)
            .IsRequired(false);
        
        builder.Property(x=>x.PaymentDate)
            .IsRequired(false);
        
        builder.Property(x=>x.PaymentDescription)
            .HasMaxLength(300)
            .IsRequired(false);
        
        builder.Property(x=>x.AmountPaid)
            .HasPrecision(18, 2)
            .HasDefaultValue(0.00);

        builder.HasIndex(x => new { x.LoanId, x.IsPaid });
        
    }
}
