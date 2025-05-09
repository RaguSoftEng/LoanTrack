using LoanTrack.Application.Loans.Queries.Installments;
using LoanTrack.Application.Loans.Queries.Reports.GetCollection;
using LoanTrack.Domain.Common;

namespace LoanTrack.Application.Loans;

public interface IInstallmentQueryRepository
{
    Task<IReadOnlyCollection<InstallmentsListResponse>> GetInstallmentsByLoanAsync(Guid loanId, CancellationToken cancellationToken = default);
    Task<GetInstallmentResponse> GetInstallmentByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<GetInstallmentResponse?> GetNextInstallmentByLoanAsync(Guid loanId, CancellationToken cancellationToken = default);
    Task<List<InstallmentsListResponse>> GetNextInstallmentsByGroupAndCenterAsync(
        Guid centerId,
        Guid groupId,
        bool includeHistory,
        CancellationToken cancellationToken = default
    );
    Task<List<InstallmentsListResponse>> GetNextInstallmentsByCustomerAsync(
        Guid customerId,
        bool includeHistory,
        CancellationToken cancellationToken = default
    );
    Task<int> GetInstallmentsCountForTodayAsync(CancellationToken cancellationToken = default);
    Task<int> GetOverDueInstallmentsCountAsync(CancellationToken cancellationToken = default);
    Task<Result<CollectionResponse>> GetCollectionAsync(
        DateOnly dateFrom,
        DateOnly dateTo,
        CancellationToken cancellationToken = default
    );
}
