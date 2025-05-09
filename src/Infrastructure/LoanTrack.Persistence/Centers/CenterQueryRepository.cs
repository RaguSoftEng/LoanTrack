using System.Globalization;
using LoanTrack.Application.Centers.Queries;
using LoanTrack.Application.Centers.Queries.Get;
using LoanTrack.Application.Centers.Queries.GetById;
using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.DTOs;
using LoanTrack.Persistence.Common.Database;
using LoanTrack.Persistence.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace LoanTrack.Persistence.Centers;

public class CenterQueryRepository(ApplicationDbContext context) : ICenterQueryRepository
{
    public async Task<List<ListValueResponse>?> ReadAsListValuesAsync(CancellationToken cancellationToken = default)
        => await context.Centers.AsNoTracking()
            .Select(x => new ListValueResponse(x.Id, x.Name, false))
            .ToListAsync(cancellationToken);

    public async Task<GetCenterByIdResponse> GetByIdAsync(Guid centerId, CancellationToken cancellationToken = default)
        => await context.Centers.AsNoTracking()
            .Where(x => x.Id == centerId)
            .Select(x => new GetCenterByIdResponse(
                x.Id,
                x.Name,
                x.Description,
                x.CenterLeaderId ?? Guid.Empty,
                x.CenterLeader != null
                    ? $"{x.CenterLeader.FullName}\n{x.CenterLeader.Nic}\n{x.CenterLeader.PhoneNumber}\n{x.CenterLeader.Address}"
                    : string.Empty
            ))
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<PaginatedResult<GetCentersResponse>> GetCentersAsync(
        QueryParameters queryParams,
        CancellationToken cancellationToken = default
    )
    {
        var query = context.Centers
            .AsNoTracking()
            .SmartSearch(queryParams.SearchBy, queryParams.Search);

        var count = await query.CountAsync(cancellationToken);

        var results = await query
            .SmartSort(queryParams.SortBy, queryParams.SortDescending)
            .SmartPaging(queryParams.Page, queryParams.PageSize)
            .Select(x => new GetCentersResponse(
                x.Id,
                x.Name,
                x.Description,
                x.CenterLeader != null
                    ? $"{x.CenterLeader.FullName} | {x.CenterLeader.Nic}"
                    : string.Empty
            ))
            .ToListAsync(cancellationToken);

        return new PaginatedResult<GetCentersResponse>
        {
            TotalCount = count,
            Page = queryParams.Page,
            PageSize = queryParams.PageSize,
            Items = results
        };
    }
}
