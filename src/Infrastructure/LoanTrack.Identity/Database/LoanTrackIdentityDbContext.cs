using LoanTrack.Identity.Configurations;
using LoanTrack.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LoanTrack.Identity.Database;

public class LoanTrackIdentityDbContext(DbContextOptions<LoanTrackIdentityDbContext> options)
    : IdentityDbContext<AppIdentityUser>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("Identity");
        builder.SeedIdentityData();
    }
}
