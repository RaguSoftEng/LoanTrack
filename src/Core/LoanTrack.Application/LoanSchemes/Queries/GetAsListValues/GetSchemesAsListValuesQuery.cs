using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.DTOs;

namespace LoanTrack.Application.LoanSchemes.Queries.GetAsListValues;

public record GetSchemesAsListValuesQuery() : IQuery<IReadOnlyCollection<ListValueResponse>>;
