using LoanTrack.Application.Common.CQRS;

namespace LoanTrack.Application.ListValues.Queries.GetListTypes;

public record GetListTypeQuery() : IQuery<IReadOnlyCollection<ListTypeResponse>>;
