using LoanTrack.Application.Common.CQRS;

namespace LoanTrack.Application.Loans.Queries.GetLoanCustomer;

public record GetLoanCustomerQuery(string Nic) : IQuery<GetLoanCustomerResponse>;
