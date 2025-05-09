using LoanTrack.Domain.Common;

namespace LoanTrack.Domain.Loans;

public interface ILoanRepository : IGenericRepository<Loan>
{
    Task<string> NextLoanNumberAsync(Guid customerId, string customerCode ,CancellationToken cancellationToken = default);
}
