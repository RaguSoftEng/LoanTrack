using LoanTrack.Application.Common.CQRS;
using LoanTrack.Domain.Common;

namespace LoanTrack.Application.Loans.Queries.Installments.GetInstallment;

public class GetInstallmentByIdQueryHandler(
    IInstallmentQueryRepository repository
): IQueryHandler<GetInstallmentByIdQuery, GetInstallmentResponse>
{
    public async Task<Result<GetInstallmentResponse>> Handle(GetInstallmentByIdQuery request,
        CancellationToken cancellationToken)
    {
        var response = await repository.GetInstallmentByIdAsync(request.Id, cancellationToken);
        return response;
    }
}
