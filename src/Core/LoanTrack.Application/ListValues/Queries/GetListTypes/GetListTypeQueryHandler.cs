using LoanTrack.Application.Common.CQRS;
using LoanTrack.Domain.Common;
using LoanTrack.Domain.ListValues;

namespace LoanTrack.Application.ListValues.Queries.GetListTypes;

public class GetListTypeQueryHandler : IQueryHandler<GetListTypeQuery, IReadOnlyCollection<ListTypeResponse>>
{
    public async Task<Result<IReadOnlyCollection<ListTypeResponse>>> Handle(GetListTypeQuery request,
        CancellationToken cancellationToken)
        => await Task.FromResult(ListTypes.GetListTypes.Select(x=> new ListTypeResponse(x)).ToList());
}
