using LoanTrack.Domain.Centers;
using LoanTrack.Domain.Customers;
using LoanTrack.Persistence.Common.Database.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanTrack.Persistence.Centers;

internal sealed class CenterConfiguration : AuditableEntityConfiguration<Center>
{
    public override void Configure(EntityTypeBuilder<Center> builder)
    {
        builder.ToTable("Centers");

        builder.Property(x => x.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.HasIndex(x => x.Name)
            .IsUnique();
        
        builder.Property(x => x.Description)
            .HasMaxLength(300)
            .IsRequired(false);
        
        builder.HasOne<Customer>(x=>x.CenterLeader)
            .WithMany()
            .HasForeignKey(x=>x.CenterLeaderId)
            .IsRequired(false);
    }
}
