namespace LoanTrack.Application.Loans.Queries.GetLoanCustomer;

public record GetLoanCustomerResponse(
    Guid CustomerId,
    string CustomerInfo,
    string LoanNumber
);
