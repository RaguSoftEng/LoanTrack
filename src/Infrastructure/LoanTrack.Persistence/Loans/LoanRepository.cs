using System.Globalization;
using LoanTrack.Domain.Loans;
using LoanTrack.Persistence.Common;
using LoanTrack.Persistence.Common.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LoanTrack.Persistence.Loans;

public class LoanRepository(
    ApplicationDbContext context
) : GenericRepository<Loan>(context), ILoanRepository
{

    public async Task<string> NextLoanNumberAsync(Guid customerId,string customerCode, CancellationToken cancellationToken = default)
    {
        string lastCode = await Context.Loans
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Where(x=>x.CustomerId == customerId)
            .OrderByDescending(x=>x.LoanNumber)
            .Select(x=>x.LoanNumber)
            .FirstOrDefaultAsync(cancellationToken);

        int nextNumber = 1;
        if (!string.IsNullOrEmpty(lastCode))
        {
            string[] parts = lastCode.Split('-');

            if (parts.Length == 3 && int.TryParse(parts[2], out int lastNumber))
            {
                nextNumber = lastNumber + 1;
            }
        }
        
        var datePart = DateTime.UtcNow.ToString("yyyyMM", CultureInfo.CurrentCulture);
        
        return $"LN{datePart}-{customerCode}-{nextNumber}";
    }
}
