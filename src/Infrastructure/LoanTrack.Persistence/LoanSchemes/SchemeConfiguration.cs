using System.Text.Json;
using LoanTrack.Domain.LoanSchemes;
using LoanTrack.Persistence.Common.Database.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanTrack.Persistence.LoanSchemes;

internal class SchemeConfiguration :  AuditableEntityConfiguration<LoanScheme>
{
    public override void Configure(EntityTypeBuilder<LoanScheme> builder)
    {
        builder.ToTable("LoanSchemes");
        
        var valueComparer = new ValueComparer<List<string>>(
            (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToList());

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.Description)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(s => s.InterestType)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(s => s.InterestRate)
            .IsRequired()
            .HasPrecision(5, 2);

        builder.Property(s => s.MinimumAmount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(s => s.MaximumAmount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(s => s.RepaymentPeriodsInMonths)
            .IsRequired();

        builder.Property(s => s.ProcessingFee)
            .HasPrecision(18, 2);
        
        builder.Property(s => s.InsuranceAmount)
            .HasPrecision(18, 2);

        builder.Property(s => s.LatePaymentPenalty)
            .HasPrecision(5, 2);

        builder.Property(s => s.IsSecuredLoan)
            .IsRequired();

        builder.Property(s => s.CollateralType)
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Property(s => s.HasFixedInterestRate)
            .IsRequired();

        builder.Property(s => s.IsGovernmentSubsidized)
            .IsRequired();

        builder.Property(s => s.RequiresGuarantor)
            .IsRequired();

        builder.Property(s => s.GracePeriodInMonths)
            .IsRequired();

        builder.Property(s => s.IsActive)
            .IsRequired();
        
        builder.Property(s => s.EligibleBorrowerTypes)
            .IsRequired(false)
            .HasColumnType("jsonb")
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null)
            ).Metadata.SetValueComparer(valueComparer);

        builder.Property(s => s.AllowedLoanPurposes)
            .IsRequired(false)
            .HasColumnType("jsonb")
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null)
            ).Metadata.SetValueComparer(valueComparer);
        
        builder.HasIndex(s => s.Name).IsUnique();
        builder.HasIndex(s => s.IsActive);
        
        
    }
}
