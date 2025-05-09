using LoanTrack.Domain.Loans;
using LoanTrack.Persistence.Common.Database;
using LoanTrack.Persistence.Common.Database.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanTrack.Persistence.Loans;

internal class LoanConfigurations : AuditableEntityConfiguration<Loan>
{
    public override void Configure(EntityTypeBuilder<Loan> builder)
    {
        builder.ToTable("Loans");
        
        builder.Property(x=>x.LoanNumber)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(x => x.LoanNumber)
            .IsUnique();

        builder.HasOne(x => x.Customer)
            .WithMany()
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        
        builder.HasOne(x=>x.LoanScheme)
            .WithMany()
            .HasForeignKey(x=>x.LoanSchemeId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);
        
        builder.Property(x=>x.LoanOfficer)
            .HasMaxLength(300)
            .IsRequired(false);
        
        builder.Property(x=>x.LoanAmount)
            .IsRequired()
            .HasPrecision(18, 2);
        
        builder.Property(x=>x.InterestType)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x=>x.InterestRate)
            .IsRequired()
            .HasPrecision(5, 2);

        builder.Property(x => x.InstallmentType)
            .HasMaxLength(20)
            .IsRequired();
        
        builder.Property(x=>x.DurationInInterestUnits)
            .IsRequired();
        
        builder.Property(x=>x.RepaymentDurations)
            .IsRequired();
        
        builder.Property(x => x.InstallmentAmount)
            .IsRequired()
            .HasPrecision(18, 2);
        
        builder.Property(x => x.IssuanceDate)
            .IsRequired(false);
        
        builder.Property(x=>x.EndDate)
            .IsRequired(false);
        
        builder.Property(x=>x.NextInstallmentDate)
            .IsRequired(false);
        
        builder.Property(x=>x.LoanDisbursementMethod)
            .HasMaxLength(30)
            .IsRequired();
        
        builder.Property(x=>x.LoanRepaymentMethod)
            .HasMaxLength(30)
            .IsRequired();
        
        builder.Property(x=>x.GuarantorsInformation)
            .HasMaxLength(500)
            .IsRequired(false);
        
        builder.Property(x => x.LoanStatus)
            .HasMaxLength(30)
            .IsRequired();
        
        builder.Property(x=>x.ClosedDate)
            .IsRequired(false);
        
        builder.Property(x=>x.TotalAmountPayable)
            .IsRequired()
            .HasPrecision(18, 2);
        
        builder.Property(x=>x.PaidAmount)
            .HasDefaultValue(0.00)
            .HasPrecision(18, 2);
        
        builder.Property(x=>x.ProcessingFee)
            .HasDefaultValue(0.00)
            .HasPrecision(18, 2);
        
        builder.Property(x=>x.InsuranceAmount)
            .HasDefaultValue(0.00)
            .HasPrecision(18, 2);
    }
}
