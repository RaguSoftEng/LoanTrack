using LoanTrack.Domain.Common.Constants;
using LoanTrack.Domain.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanTrack.Persistence.Employees;

public class EmployeeConfiguration: IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");
        
        builder.Property(x=>x.FirstName)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(x=>x.LastName)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(x=>x.Email)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(x=>x.Role)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(e => e.IsActive)
            .IsRequired();
        builder.HasIndex(e => e.IsActive);
        
        builder.HasIndex(x=>x.Email).IsUnique();
        builder.HasIndex(x=>x.IdentityId).IsUnique();

        var employee = Employee.Create(
            "System",
            "Admin",
            "admin@email.com",
            "09c16865-ee5a-466f-aff4-acbd5eaf8dd8",
            EmployeeRoles.Admin
        );
        builder.HasData(employee);
    }
}
