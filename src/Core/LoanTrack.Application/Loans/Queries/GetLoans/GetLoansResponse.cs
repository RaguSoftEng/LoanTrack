namespace LoanTrack.Application.Loans.Queries.GetLoans;

public record GetLoansResponse(
    Guid LoanId,
    string LoanNumber,
    string Customer,
    double LoanAmount,
    double Balance,
    DateOnly? IssuanceDate,
    DateOnly? EndDate,
    string Status
);
