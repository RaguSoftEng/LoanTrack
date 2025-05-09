using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.DTOs;

namespace LoanTrack.Application.Groups.Queries.GetGroupsListValues;

public record GetGroupsListValueQuery(Guid CenterId) : IQuery<IReadOnlyCollection<ListValueResponse>>;
