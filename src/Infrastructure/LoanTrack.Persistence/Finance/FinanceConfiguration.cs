using LoanTrack.Domain.Finance;
using LoanTrack.Persistence.Common.Database.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanTrack.Persistence.Finance;

internal sealed class FinanceConfiguration : AuditableEntityConfiguration<FinanceJournal>
{
    public override void Configure(EntityTypeBuilder<FinanceJournal> builder)
    {
        builder.ToTable("FinanceJournals");

        builder.Property(x => x.JournalDate)
            .IsRequired();
        
        builder.Property(x => x.JournalType)
            .HasMaxLength(30)
            .IsRequired();
        
        builder.Property(x=>x.ReferenceType)
            .HasMaxLength(30)
            .IsRequired();
        
        builder.Property(x=>x.ReferenceId)
            .IsRequired();
        
        builder.Property(x=> x.Amount)
            .HasPrecision(18, 2)
            .IsRequired();
    }
}
