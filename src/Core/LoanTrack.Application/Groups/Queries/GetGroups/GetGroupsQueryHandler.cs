using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.DTOs;
using LoanTrack.Domain.Common;

namespace LoanTrack.Application.Groups.Queries.GetGroups;

public sealed class GetGroupsQueryHandler(IGroupQueryRepository groupRead)
    : IQueryHandler<GetGroupsQuery, PaginatedResult<GetGroupsResponse>>
{
    public async Task<Result<PaginatedResult<GetGroupsResponse>>> Handle(GetGroupsQuery request, CancellationToken cancellationToken)
    {
        var response = await groupRead.GetGroupsByFilterAsync(
            request.ToBaseQuery(),
            request.Center,
            cancellationToken
        );

        return response;
    }
}
