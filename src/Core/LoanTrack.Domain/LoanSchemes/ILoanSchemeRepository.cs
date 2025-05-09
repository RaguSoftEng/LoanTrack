using LoanTrack.Domain.Common;

namespace LoanTrack.Domain.LoanSchemes;

public interface ILoanSchemeRepository : IGenericRepository<LoanScheme>
{
    Task<string> GenerateNextSchemeCodeAsync(CancellationToken cancellationToken = default);
}
