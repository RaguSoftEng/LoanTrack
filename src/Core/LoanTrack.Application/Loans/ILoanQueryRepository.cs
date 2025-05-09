using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.DTOs;
using LoanTrack.Application.Dashboard;
using LoanTrack.Application.Loans.Queries.GetLoanById;
using LoanTrack.Application.Loans.Queries.GetLoanCustomer;
using LoanTrack.Application.Loans.Queries.GetLoans;
using LoanTrack.Application.Loans.Queries.Installments;
using LoanTrack.Domain.Common;

namespace LoanTrack.Application.Loans;

public interface ILoanQueryRepository
{
    Task<GetLoanCustomerResponse?> GetLoanCustomerInfoAsync(string nic, CancellationToken cancellationToken = default);

    Task<PaginatedResult<GetLoansResponse>> GetLoansByFilterAsync(
        QueryParameters parameters,
        Guid centerId,
        Guid groupId,
        string nic,
        CancellationToken cancellationToken = default
    );
    Task<GetLoanByIdResponse?> GetLoanByIdAsync(Guid loanId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<(string Status, int Count)>> GetLoanCountsByStatusAsync(
        CancellationToken cancellationToken = default
    );
}
