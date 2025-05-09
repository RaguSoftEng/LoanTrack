using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.DTOs;
using LoanTrack.Domain.Common;

namespace LoanTrack.Application.ListValues.Queries.GetListValuesByType;

public class GetListValuesByTypeQueryHandler(
    IListValueQueryRepository listValueQueryRepository
) : IQueryHandler<GetListValuesByTypeQuery, IReadOnlyCollection<ListValueResponse>>
{
    public async Task<Result<IReadOnlyCollection<ListValueResponse>>> Handle(
        GetListValuesByTypeQuery request,
        CancellationToken cancellationToken
    ) => await listValueQueryRepository.GetValuesByListAsync(request.ListType, request.ParentId, cancellationToken);
}
