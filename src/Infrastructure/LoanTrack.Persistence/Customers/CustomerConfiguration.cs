using LoanTrack.Domain.Customers;
using LoanTrack.Domain.ListValues;
using LoanTrack.Persistence.Common.Database;
using LoanTrack.Persistence.Common.Database.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanTrack.Persistence.Customers;

internal sealed class CustomerConfiguration : AuditableEntityConfiguration<Customer>
{
    public override void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");
        
        builder.Property(c => c.Code)
            .HasDefaultValueSql($"nextval('{Constants.SchemaName}.{Constants.CodeSeqStartOneIncOne}')");

        builder.HasIndex(x => x.Code)
            .IsUnique();
        
        builder.HasIndex(x=>x.Nic)
            .IsUnique();
        
        builder.Property(x => x.Nic)
            .IsRequired()
            .HasMaxLength(15);
        
        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(x => x.Gender)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(x => x.Email)
            .IsRequired(false)
            .HasMaxLength(50);

        builder.Property(x => x.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.Address)
            .IsRequired()
            .HasMaxLength(300);
        
        builder.HasOne(x=>x.GramaNiladhari)
            .WithMany()
            .HasForeignKey(x=>x.GramaNiladhariId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);

        builder.HasOne<ListValue>(x => x.DsDivision)
            .WithMany()
            .HasForeignKey(x => x.DsDivisionId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);

        builder.HasOne(x => x.District)
            .WithMany()
            .HasForeignKey(x => x.DistrictId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);
        
        builder.HasOne(x => x.Province)
            .WithMany()
            .HasForeignKey(x => x.ProvinceId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);
        
        builder.HasOne(x => x.Center)
            .WithMany()
            .HasForeignKey(x => x.CenterId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasOne(x => x.Group)
            .WithMany()
            .HasForeignKey(x => x.GroupId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);

        builder.Property(x => x.Occupation)
            .HasMaxLength(150)
            .IsRequired(false);

        builder.Property(x => x.DateOfBirth)
            .IsRequired();
        
        builder.Property(x => x.BankName)
            .HasMaxLength(100)
            .IsRequired(false);
        
        builder.Property(x => x.BankBranch)
            .HasMaxLength(100)
            .IsRequired(false);
        
        builder.Property(x => x.BankAccountNumber)
            .HasMaxLength(50)
            .IsRequired(false);
        
        builder.Property(x => x.AccountName)
            .HasMaxLength(100)
            .IsRequired(false);
        
        builder.Property(x => x.WorkAddress)
            .HasMaxLength(300)
            .IsRequired(false);
    }
}
