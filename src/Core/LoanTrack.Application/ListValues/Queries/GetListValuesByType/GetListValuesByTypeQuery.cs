using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.DTOs;

namespace LoanTrack.Application.ListValues.Queries.GetListValuesByType;

public record GetListValuesByTypeQuery(string ListType, Guid ParentId) : IQuery<IReadOnlyCollection<ListValueResponse>>;
