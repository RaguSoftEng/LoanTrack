using LoanTrack.Application.Common.CQRS;
using LoanTrack.Domain.Common;

namespace LoanTrack.Application.Groups.Queries.GetGroupById;

public class GetGroupByIdQueryHandler(IGroupQueryRepository groupRead)
    : IQueryHandler<GetGroupByIdQuery, GetGroupByIdResponse>
{
    public async Task<Result<GetGroupByIdResponse>> Handle(GetGroupByIdQuery request, CancellationToken cancellationToken)
    {
        var response = await groupRead.GetByIdAsync(request.GroupId, cancellationToken);

        return response;
    }
}
