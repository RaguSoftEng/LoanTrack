using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.DTOs;
using LoanTrack.Application.Customers.Queries.GetCustomer;

namespace LoanTrack.Application.Customers.Queries.GetCustomers;

public record GetCustomersQuery : QueryParameters, IQuery<PaginatedResult<CustomerResponse>>
{
    public Guid CenterId { get; init; }
    public Guid GroupId { get; init; }
}
