using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.DTOs;
using LoanTrack.Domain.Common;
using LoanTrack.Domain.Groups;

namespace LoanTrack.Application.Groups.Queries.GetGroupsListValues;

public class GetGroupsListValueHandler(IGroupQueryRepository groupRead)
    : IQueryHandler<GetGroupsListValueQuery, IReadOnlyCollection<ListValueResponse>>
{
    public async Task<Result<IReadOnlyCollection<ListValueResponse>>> Handle(GetGroupsListValueQuery request, CancellationToken cancellationToken)
    {
        var response = await groupRead.ReadAsListValueByCenterAsync(request.CenterId, cancellationToken);

        return response ?? Result.Failure<IReadOnlyCollection<ListValueResponse>>(GroupErrors.NotFound());
    }
}
