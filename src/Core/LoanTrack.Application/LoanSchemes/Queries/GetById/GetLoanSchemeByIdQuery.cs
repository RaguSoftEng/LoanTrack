using LoanTrack.Application.Common.CQRS;

namespace LoanTrack.Application.LoanSchemes.Queries.GetById;

public record GetLoanSchemeByIdQuery(Guid Id) : IQuery<LoanSchemeResponse>;
