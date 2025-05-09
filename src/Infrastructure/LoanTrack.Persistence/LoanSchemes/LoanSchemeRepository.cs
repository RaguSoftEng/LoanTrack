using LoanTrack.Domain.LoanSchemes;
using LoanTrack.Persistence.Common;
using LoanTrack.Persistence.Common.Database;
using Microsoft.EntityFrameworkCore;

namespace LoanTrack.Persistence.LoanSchemes;

public class LoanSchemeRepository(ApplicationDbContext context)
    : GenericRepository<LoanScheme>(context), ILoanSchemeRepository
{
    public async Task<string> GenerateNextSchemeCodeAsync(CancellationToken cancellationToken = default)
    {
        var lastScheme = await Context.LoanSchemes
            .AsNoTracking()
            .IgnoreQueryFilters()
            .OrderByDescending(s => s.Code)
            .Select(x=>x.Code)
            .FirstOrDefaultAsync(cancellationToken);

        int nextNumber = 1;

        if (lastScheme != null)
        {
            string[] parts = lastScheme.Split('-');

            if (parts.Length == 2 && int.TryParse(parts[1], out int lastNumber))
            {
                nextNumber = lastNumber + 1;
            }
        }

        return $"SCHEME-{nextNumber}";
    }
}
