using LoanTrack.Application.Common.CQRS;
using LoanTrack.Domain.Common;
using LoanTrack.Domain.Customers;

namespace LoanTrack.Application.Customers.Queries.GetCustomer.ById;

public class GetCustomerByIdQueryHandler(ICustomerQueryRepository queryRepository)
    : IQueryHandler<GetCustomerByIdQuery, CustomerResponse>
{
    public async Task<Result<CustomerResponse>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var response = await queryRepository.GetByIdAsync(request.CustomerId, cancellationToken);

        return response ?? Result.Failure<CustomerResponse>(CustomerErrors.NotFound(request.CustomerId));
    }
}
