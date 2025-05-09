using LoanTrack.Application.Common.CQRS;

namespace LoanTrack.Application.Finance.Reports.GetFinanceSummary;

public record GetFinanceSummaryQuery(DateOnly StartDate, DateOnly EndDate) : IQuery<FinanceSummaryResponse>;
