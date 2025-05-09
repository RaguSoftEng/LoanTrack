using LoanTrack.Application.Common.CQRS;
using LoanTrack.Domain.Common;

namespace LoanTrack.Application.Finance.Reports.LoanSummary;

public class GetLoanSummaryQueryHandler(
    IFinanceJournalQueryRepository repository
):IQueryHandler<GetLoanSummaryQuery, IReadOnlyList<LoanSummaryResponse>>
{
    public Task<Result<IReadOnlyList<LoanSummaryResponse>>> Handle(
        GetLoanSummaryQuery request,
        CancellationToken cancellationToken
    ) => repository.GetLoanSummaryAsync(
        request.CenterId,
        request.GroupId,
        request.Nic,
        request.IncludeClosed,
        cancellationToken
    );
}
