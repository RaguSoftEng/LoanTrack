using LoanTrack.Application.Common.CQRS;
using LoanTrack.Domain.Common;

namespace LoanTrack.Application.Centers.Queries.GetById;

public class GetCenterByIdQueryHandler(ICenterQueryRepository centerQuery)
    : IQueryHandler<GetCenterByIdQuery, GetCenterByIdResponse>
{
    public async Task<Result<GetCenterByIdResponse>> Handle(GetCenterByIdQuery request, CancellationToken cancellationToken)
    {
        var response = await centerQuery.GetByIdAsync(request.CenterId, cancellationToken);

        return response;
    }
}
