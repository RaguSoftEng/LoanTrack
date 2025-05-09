using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.DTOs;
using LoanTrack.Application.Customers.Queries.GetCustomer;
using LoanTrack.Domain.Common;

namespace LoanTrack.Application.Customers.Queries.GetCustomers;

public class GetCustomersQueryHandler(ICustomerQueryRepository queryRepository)
    : IQueryHandler<GetCustomersQuery, PaginatedResult<CustomerResponse>>
{
    public async Task<Result<PaginatedResult<CustomerResponse>>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        var response = await queryRepository.GetCustomersByFilter(
            request.ToBaseQuery(),
            request.CenterId,
            request.GroupId,
            cancellationToken
        );

        return response;
    }
}
