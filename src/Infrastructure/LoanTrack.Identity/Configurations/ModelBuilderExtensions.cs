using LoanTrack.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LoanTrack.Identity.Configurations;

internal static class ModelBuilderExtensions
{
    public static void SeedIdentityData(this ModelBuilder builder)
    {
        string adminId = new Guid("09c16865-ee5a-466f-aff4-acbd5eaf8dd8").ToString();
        string adminRoleId = new Guid("f3997dba-8bdb-4a90-867a-087f3dd71e87").ToString();
        string managerRoleId = new Guid("84dd2374-ad81-4376-8704-ff81a47651fb").ToString();
        string employeeRoleId = new Guid("fdd465ff-32b3-4a4a-9c0f-00e39900b0c9").ToString();
        var hasher = new PasswordHasher<AppIdentityUser>();

        builder.Entity<IdentityRole>()
            .HasData(
                new IdentityRole
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = managerRoleId,
                    Name = "Manager",
                    NormalizedName = "MANAGER"
                },
                new IdentityRole
                {
                    Id = employeeRoleId,
                    Name = "Employee",
                    NormalizedName = "EMPLOYEE"
                }
            );

        var adminUser = new AppIdentityUser
        {
            Id = adminId,
            UserName = "admin@email.com",
            NormalizedUserName = "ADMIN@EMAIL.COM",
            Firstname = "System",
            Lastname = "Admin",
            Email = "admin@email.com",
            NormalizedEmail = "ADMIN@EMAIL.COM",
            EmailConfirmed = true
        };
        adminUser.PasswordHash = hasher.HashPassword(adminUser, "P@ssw0rd");

        builder.Entity<AppIdentityUser>()
            .HasData(adminUser);

        builder.Entity<IdentityUserRole<string>>()
            .HasData(
                new IdentityUserRole<string>
                {
                    RoleId = adminRoleId,
                    UserId = adminId
                }
            );
    }
}
