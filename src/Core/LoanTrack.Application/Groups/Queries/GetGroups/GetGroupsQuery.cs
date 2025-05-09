using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.DTOs;

namespace LoanTrack.Application.Groups.Queries.GetGroups;

public record GetGroupsQuery : QueryParameters, IQuery<PaginatedResult<GetGroupsResponse>>
{
    public Guid Center { get; init; }
}
