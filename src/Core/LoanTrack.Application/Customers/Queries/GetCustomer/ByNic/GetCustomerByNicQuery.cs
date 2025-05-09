using LoanTrack.Application.Common.CQRS;

namespace LoanTrack.Application.Customers.Queries.GetCustomer.ByNic;

public sealed record GetCustomerByNicQuery(string Nic): IQuery<CustomerResponse>;
