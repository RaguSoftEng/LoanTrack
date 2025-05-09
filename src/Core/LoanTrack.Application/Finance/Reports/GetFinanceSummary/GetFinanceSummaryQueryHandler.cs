using LoanTrack.Application.Common.CQRS;
using LoanTrack.Domain.Common;

namespace LoanTrack.Application.Finance.Reports.GetFinanceSummary;

public class GetFinanceSummaryQueryHandler(
    IFinanceJournalQueryRepository repository
): IQueryHandler<GetFinanceSummaryQuery, FinanceSummaryResponse>
{
    public Task<Result<FinanceSummaryResponse>> Handle(
        GetFinanceSummaryQuery request,
        CancellationToken cancellationToken
    ) => repository.GetFinanceSummaryAsync(request.StartDate, request.EndDate, cancellationToken);
}
