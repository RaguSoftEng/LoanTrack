using LoanTrack.Application.Common.CQRS;

namespace LoanTrack.Application.LoanSchemes.Queries.GetList;

public record GetAllLoanSchemesQuery() : IQuery<IReadOnlyCollection<LoanSchemeListResponse>>;
