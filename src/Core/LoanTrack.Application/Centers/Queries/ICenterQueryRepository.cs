using LoanTrack.Application.Centers.Queries.Get;
using LoanTrack.Application.Centers.Queries.GetById;
using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.DTOs;

namespace LoanTrack.Application.Centers.Queries;

public interface ICenterQueryRepository
{
    Task<List<ListValueResponse>?> ReadAsListValuesAsync(CancellationToken cancellationToken = default);
    Task<GetCenterByIdResponse> GetByIdAsync(Guid centerId ,CancellationToken cancellationToken = default);
    Task<PaginatedResult<GetCentersResponse>> GetCentersAsync(
        QueryParameters queryParams,
        CancellationToken cancellationToken = default
    );
}
