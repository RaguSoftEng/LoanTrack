using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.DTOs;

namespace LoanTrack.Application.Centers.Queries.GetAsListValue;

public record GetCentersListValueQuery() : IQuery<IReadOnlyCollection<ListValueResponse>>;
