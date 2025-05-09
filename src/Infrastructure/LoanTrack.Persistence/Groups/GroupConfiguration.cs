using LoanTrack.Domain.Centers;
using LoanTrack.Domain.Groups;
using LoanTrack.Persistence.Common.Database.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanTrack.Persistence.Groups;

internal sealed class GroupConfiguration : AuditableEntityConfiguration<CustomerGroup>
{
    public override void Configure(EntityTypeBuilder<CustomerGroup> builder)
    {
        builder.ToTable("CustomerGroups");

        builder.Property(x => x.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.HasIndex(x => new { x.Name, x.CenterId })
            .IsUnique();
        
        builder.Property(x => x.Description)
            .HasMaxLength(300)
            .IsRequired(false);
        
        builder.HasOne<Center>(x=>x.Center)
            .WithMany()
            .HasForeignKey(x=>x.CenterId)
            .IsRequired();
    }
}
