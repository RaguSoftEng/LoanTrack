using LoanTrack.Application.Common.CQRS;
using LoanTrack.Domain.Common;
using LoanTrack.Domain.Customers;

namespace LoanTrack.Application.Customers.Queries.GetCustomer.ByNic;

public class GetCustomerByNicQueryHandler(ICustomerQueryRepository queryRepository)
    : IQueryHandler<GetCustomerByNicQuery, CustomerResponse>
{
    public async Task<Result<CustomerResponse>> Handle(GetCustomerByNicQuery request, CancellationToken cancellationToken)
    {

        var response = await queryRepository.GetByNicAsync(request.Nic, cancellationToken);

        if (response is null)
        {
            return Result.Failure<CustomerResponse>(CustomerErrors.NotFound(request.Nic));
        }

        return response;
    }
}
