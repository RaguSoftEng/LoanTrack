using System.Security.Claims;
using LoanTrack.Application.Common.Data;
using LoanTrack.Domain.Centers;
using LoanTrack.Domain.Common;
using LoanTrack.Domain.Customers;
using LoanTrack.Domain.Employees;
using LoanTrack.Domain.Finance;
using LoanTrack.Domain.Groups;
using LoanTrack.Domain.ListValues;
using LoanTrack.Domain.Loans;
using LoanTrack.Domain.LoanSchemes;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace LoanTrack.Persistence.Common.Database;

public sealed class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    IHttpContextAccessor httpContextAccessor,
    IMemoryCache memoryCache
) : DbContext(options), IUnitOfWork
{
    internal DbSet<Customer> Customers { get; set; }
    internal DbSet<CustomerGroup> CustomerGroups { get; set; }
    internal DbSet<ListValue> ListValues { get; set; }
    internal DbSet<Employee> Users { get; set; }
    internal DbSet<LoanScheme> LoanSchemes { get; set; }
    internal DbSet<Center> Centers { get; set; }
    internal DbSet<Loan> Loans { get; set; }
    internal DbSet<LoanInstallment> LoanInstallments { get; set; }
    internal DbSet<FinanceJournal> FinanceJournals { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasSequence<int>(Constants.CodeSeqStartOneIncOne)
            .StartsAt(1)
            .IncrementsBy(1);
        modelBuilder.HasDefaultSchema(Constants.SchemaName);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var userId = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Guid employeeId = Guid.Empty;

        var entries = ChangeTracker.Entries<AuditableEntity>()
            .Where(x => x.State is EntityState.Added or EntityState.Modified)
            .ToList();

        if (entries.Any() && !string.IsNullOrWhiteSpace(userId))
        {
            employeeId = await memoryCache.GetOrCreateAsync(userId, async entry =>
            {
                Guid? response = await Users
                    .AsNoTracking()
                    .Where(x => x.IdentityId == userId)
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync(cancellationToken);
                entry.SetAbsoluteExpiration(TimeSpan.FromDays(30));
                entry.SetPriority(CacheItemPriority.High);
                return response;
            }) ?? Guid.Empty;

        }
        
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Modified)
            {
                if (entry.Entity.IsDeleted)
                {
                    entry.Entity.DeletedAt = DateTime.UtcNow;
                }
                else
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
                entry.Entity.UpdatedBy = employeeId;
            }
            else
            {
                entry.Entity.CreatedBy = employeeId;
            }
        }
        
        return await base.SaveChangesAsync(cancellationToken);
    }
}
