using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.DTOs;
using LoanTrack.Domain.Common;
using LoanTrack.Domain.Groups;

namespace LoanTrack.Application.Centers.Queries.GetAsListValue;

public class GetCentersListValueHandler(ICenterQueryRepository centerQuery)
    : IQueryHandler<GetCentersListValueQuery, IReadOnlyCollection<ListValueResponse>>
{
    public async Task<Result<IReadOnlyCollection<ListValueResponse>>> Handle(GetCentersListValueQuery request, CancellationToken cancellationToken)
    {
        var response = await centerQuery.ReadAsListValuesAsync(cancellationToken);

        return response ?? Result.Failure<IReadOnlyCollection<ListValueResponse>>(GroupErrors.NotFound());
    }
}
