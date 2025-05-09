using LoanTrack.Application.Common.CQRS;
using LoanTrack.Domain.Common;

namespace LoanTrack.Application.LoanSchemes.Queries.GetList;

public class GetAllLoanSchemesQueryHandler(
    ILoanSchemeQueryRepository repository
): IQueryHandler<GetAllLoanSchemesQuery, IReadOnlyCollection<LoanSchemeListResponse>>
{
    public async Task<Result<IReadOnlyCollection<LoanSchemeListResponse>>> Handle(GetAllLoanSchemesQuery request,
        CancellationToken cancellationToken)
        => await repository.GetListAsync(cancellationToken);
}
