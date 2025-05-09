using LoanTrack.Application.Common.DTOs;
using LoanTrack.Application.LoanSchemes.Queries;
using LoanTrack.Application.LoanSchemes.Queries.GetList;
using LoanTrack.Domain.LoanSchemes;
using LoanTrack.Persistence.Common.Database;
using Microsoft.EntityFrameworkCore;

namespace LoanTrack.Persistence.LoanSchemes;

public class LoanSchemeQueryRepository(ApplicationDbContext context) : ILoanSchemeQueryRepository
{
    public async Task<LoanScheme> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await context.LoanSchemes.Where(x => x.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<List<LoanScheme>> GetAllAsync(CancellationToken cancellationToken = default)
        => await context.LoanSchemes.AsNoTracking().ToListAsync(cancellationToken: cancellationToken);

    public async Task<List<LoanSchemeListResponse>> GetListAsync(CancellationToken cancellationToken = default)
        => await context.LoanSchemes
            .Select(x=> new LoanSchemeListResponse(
                x.Id,
                x.Code,
                x.Name,
                x.InterestRate,
                x.MinimumAmount,
                x.MinimumAmount,
                x.RepaymentPeriodsInMonths,
                x.HasFixedInterestRate,
                x.RequiresGuarantor,
                x.GracePeriodInMonths,
                x.IsActive
            ))
            .AsNoTracking().ToListAsync(cancellationToken: cancellationToken);

    public async Task<List<ListValueResponse>> GetListValuesAsync(CancellationToken cancellationToken = default)
        => await context.LoanSchemes.AsNoTracking()
            .Select(x=> new ListValueResponse(x.Id, $"{x.Code}|{x.Name}", false))
            .ToListAsync(cancellationToken: cancellationToken);
}
