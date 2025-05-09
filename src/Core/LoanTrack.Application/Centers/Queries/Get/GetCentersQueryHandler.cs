using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.DTOs;
using LoanTrack.Domain.Common;

namespace LoanTrack.Application.Centers.Queries.Get;

public class GetCentersQueryHandler(ICenterQueryRepository centerQuery)
    : IQueryHandler<GetCentersQuery, PaginatedResult<GetCentersResponse>>
{
    public async Task<Result<PaginatedResult<GetCentersResponse>>> Handle(GetCentersQuery request, CancellationToken cancellationToken)
    {
        var response = await centerQuery.GetCentersAsync(
            request.ToBaseQuery(),
            cancellationToken
        );

        return response;
    }
}
