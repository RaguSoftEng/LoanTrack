using LoanTrack.Application.Common.CQRS;
using LoanTrack.Domain.Common;

namespace LoanTrack.Application.Loans.Queries.Installments.GetNextInstallments;

public class GetNextInstallmentsQueryHandler(
    IInstallmentQueryRepository repository
): IQueryHandler<GetNextInstallmentsQuery, IReadOnlyCollection<InstallmentsListResponse>>
{
    public async Task<Result<IReadOnlyCollection<InstallmentsListResponse>>> Handle(
        GetNextInstallmentsQuery request,
        CancellationToken cancellationToken
    )
    {
        
        var centerId = request.CenterId ?? Guid.Empty;
        var groupId = request.GroupId ?? Guid.Empty;
        
        return await repository.GetNextInstallmentsByGroupAndCenterAsync(centerId, groupId, false, cancellationToken);
    }
}
