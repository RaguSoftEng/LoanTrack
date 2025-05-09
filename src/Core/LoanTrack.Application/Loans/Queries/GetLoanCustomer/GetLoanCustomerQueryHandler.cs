using LoanTrack.Application.Common.CQRS;
using LoanTrack.Domain.Common;
using LoanTrack.Domain.Customers;

namespace LoanTrack.Application.Loans.Queries.GetLoanCustomer;

public class GetLoanCustomerQueryHandler(ILoanQueryRepository repository)
: IQueryHandler<GetLoanCustomerQuery, GetLoanCustomerResponse>
{
    public async Task<Result<GetLoanCustomerResponse>> Handle(GetLoanCustomerQuery request,
        CancellationToken cancellationToken)
    {
        var result = await repository.GetLoanCustomerInfoAsync(request.Nic, cancellationToken);
        return result is null ?
            Result.Failure<GetLoanCustomerResponse>(CustomerErrors.NotFound(nic: request.Nic))
            : Result.Success(result);
    }
}
