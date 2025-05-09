using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.DTOs;
using LoanTrack.Domain.Common;

namespace LoanTrack.Application.LoanSchemes.Queries.GetAsListValues;

public class GetSchemesAsListValuesQueryHandler(
    ILoanSchemeQueryRepository repository
) : IQueryHandler<GetSchemesAsListValuesQuery, IReadOnlyCollection<ListValueResponse>>
{
    public async Task<Result<IReadOnlyCollection<ListValueResponse>>> Handle(GetSchemesAsListValuesQuery request,
        CancellationToken cancellationToken)
    {
        return await repository.GetListValuesAsync(cancellationToken);
    }
}
