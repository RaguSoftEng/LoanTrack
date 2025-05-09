using LoanTrack.Application.Common.CQRS;
using LoanTrack.Domain.Common;

namespace LoanTrack.Application.Loans.Queries.Installments.GetNextInstallment;

public class GetNextInstallmentQueryHandler(
    IInstallmentQueryRepository repository
): IQueryHandler<GetNextInstallmentQuery, GetInstallmentResponse>
{
    public async Task<Result<GetInstallmentResponse>> Handle(GetNextInstallmentQuery request,
        CancellationToken cancellationToken)
        => await repository.GetNextInstallmentByLoanAsync(request.LoanId, cancellationToken);
}
