using LoanTrack.Domain.ListValues;
using LoanTrack.Persistence.Common.Database.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanTrack.Persistence.ListValues;

public class ListValueConfiguration : BaseEntityConfiguration<ListValue>
{
    public override void Configure(EntityTypeBuilder<ListValue> builder)
    {
        builder.ToTable("ListValues");
        builder.Property(x=>x.ListType)
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(x=>x.Description)
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(x=>x.ParentId)
            .HasDefaultValue(Guid.Empty)
            .IsRequired();
        builder.HasIndex(x=> new {x.ListType, x.Description, x.ParentId})
            .IsUnique();
    }
}
