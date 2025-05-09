using LoanTrack.Application.Common.CQRS;
using LoanTrack.Domain.Common;

namespace LoanTrack.Application.Loans.Queries.Installments.GetInstallments;

public class GetInstallmentsQueryHandler(
    IInstallmentQueryRepository repository
): IQueryHandler<GetInstallmentsQuery, IReadOnlyCollection<InstallmentsListResponse>>
{
    public async Task<Result<IReadOnlyCollection<InstallmentsListResponse>>> Handle(GetInstallmentsQuery request,
        CancellationToken cancellationToken)
    {
        var response = await repository.GetInstallmentsByLoanAsync(request.LoanId, cancellationToken);
        return Result.Success(response);
    }
}
