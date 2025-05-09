using LoanTrack.Domain.Common;
using LoanTrack.Domain.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanTrack.Persistence.Common.Database.Configurations;

internal class AuditableEntityConfiguration<T> : BaseEntityConfiguration<T> where T : AuditableEntity
{
    public override void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property(e => e.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");
        
        builder.Property(e => e.UpdatedAt)
            .IsRequired(false);
        
        builder.Property(e => e.DeletedAt)
            .IsRequired(false);
        
        builder.Property(e => e.IsDeleted)
            .HasDefaultValue(false);
        builder.HasIndex(e => e.IsDeleted)
            .HasFilter("[IsDeleted] = 0");
        
        builder.HasOne<Employee>()
            .WithMany()
            .HasForeignKey(e => e.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        
        builder.HasOne<Employee>()
            .WithMany()
            .HasForeignKey(e => e.UpdatedBy)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);
        
        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
