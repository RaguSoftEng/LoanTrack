using LoanTrack.Application.Common.CQRS;

namespace LoanTrack.Application.Finance.Reports.LoanSummary;

public record GetLoanSummaryQuery(
    Guid CenterId,
    Guid GroupId,
    string Nic,
    bool IncludeClosed
) : IQuery<IReadOnlyList<LoanSummaryResponse>>;
