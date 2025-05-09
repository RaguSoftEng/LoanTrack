using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.DTOs;
using LoanTrack.Application.Groups;
using LoanTrack.Application.Groups.Queries;
using LoanTrack.Application.Groups.Queries.GetGroupById;
using LoanTrack.Application.Groups.Queries.GetGroups;
using LoanTrack.Persistence.Common.Database;
using LoanTrack.Persistence.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace LoanTrack.Persistence.Groups;

public class GroupQueryRepository(ApplicationDbContext context) : IGroupQueryRepository
{
    public async Task<List<ListValueResponse>?> ReadAsListValueByCenterAsync(
        Guid centerId,
        CancellationToken cancellationToken = default
    ) => await context.CustomerGroups
            .Where(g => g.CenterId == centerId)
            .Select(x => new ListValueResponse(x.Id, x.Name, false)
            ).AsNoTracking()
            .ToListAsync(cancellationToken);

    public async Task<List<GetGroupsResponse>> GetByCenterAsync(Guid centerId,
        CancellationToken cancellationToken = default)
        => await context.CustomerGroups.Where(g => centerId == Guid.Empty || g.CenterId == centerId)
            .Select(x => new GetGroupsResponse(
                x.Id,
                x.Name,
                x.Description,
                x.Center.Name
            )).AsNoTracking()
            .ToListAsync(cancellationToken);

    public async Task<GetGroupByIdResponse> GetByIdAsync(Guid groupId, CancellationToken cancellationToken = default)
        => await context.CustomerGroups.Where(g => g.Id == groupId)
            .Select(x=> new GetGroupByIdResponse(
                    x.Id,
                    x.Name,
                    x.Description,
                    x.Center.Name,
                    x.CenterId
                ))
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<PaginatedResult<GetGroupsResponse>> GetGroupsByFilterAsync(
        QueryParameters queryParams,
        Guid centerId,
        CancellationToken cancellationToken = default
    )
    {
        var query = context.CustomerGroups
            .AsNoTracking()
            .WhereIf(centerId != Guid.Empty, x => x.CenterId == centerId)
            .SmartSearch(queryParams.SearchBy, queryParams.Search);

        var totalCount = await query.CountAsync(cancellationToken);

        var results = await query
            .SmartSort(queryParams.SortBy, queryParams.SortDescending)
            .SmartPaging(queryParams.Page, queryParams.PageSize)
            .Select(x => new GetGroupsResponse(
                x.Id,
                x.Name,
                x.Description,
                x.Center.Name
            ))
            .ToListAsync(cancellationToken);

        return new PaginatedResult<GetGroupsResponse>
        {
            TotalCount = totalCount,
            Items = results,
            Page = queryParams.Page,
            PageSize = queryParams.PageSize
        };
    }
}
