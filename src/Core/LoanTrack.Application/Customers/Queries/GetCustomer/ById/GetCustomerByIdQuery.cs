using LoanTrack.Application.Common.CQRS;

namespace LoanTrack.Application.Customers.Queries.GetCustomer.ById;

public sealed record GetCustomerByIdQuery(Guid CustomerId) : IQuery<CustomerResponse>;

