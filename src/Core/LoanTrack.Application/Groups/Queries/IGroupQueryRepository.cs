using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.DTOs;
using LoanTrack.Application.Groups.Queries.GetGroupById;
using LoanTrack.Application.Groups.Queries.GetGroups;

namespace LoanTrack.Application.Groups.Queries;

public interface IGroupQueryRepository
{
    Task<List<ListValueResponse>?> ReadAsListValueByCenterAsync(Guid centerId, CancellationToken cancellationToken = default);
    Task<List<GetGroupsResponse>> GetByCenterAsync(Guid centerId, CancellationToken cancellationToken = default);
    Task<GetGroupByIdResponse> GetByIdAsync(Guid groupId, CancellationToken cancellationToken = default);
    Task<PaginatedResult<GetGroupsResponse>> GetGroupsByFilterAsync(QueryParameters queryParams, Guid centerId, CancellationToken cancellationToken = default);
}
