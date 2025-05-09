using LoanTrack.Application.Finance.Reports.GetFinanceSummary;
using LoanTrack.Application.Finance.Reports.LoanSummary;
using LoanTrack.Domain.Common;

namespace LoanTrack.Application.Finance;

public interface IFinanceJournalQueryRepository
{
    Task<Result<FinanceSummaryResponse>> GetFinanceSummaryAsync(
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default
    );
    
    /*Task<Result<FinancialPositionResponse>> GetFinancialPositionAsync(
        DateOnly asOfDate,
        CancellationToken cancellationToken = default
    );*/
    
    Task<Result<IReadOnlyList<LoanSummaryResponse>>> GetLoanSummaryAsync(
        Guid centerId,
        Guid groupId,
        string nic,
        bool includeClosed,
        CancellationToken cancellationToken = default
    );
}
