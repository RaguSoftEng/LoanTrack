namespace LoanTrack.Application.Loans.Queries.GetLoanById;

public record GetLoanByIdResponse(
    Guid LoanId,
    string LoanNumber,
    string Customer,
    string LoanScheme,
    Guid? SchemeId,
    string LoanOfficer,
    double LoanAmount,
    string InterestType,
    double InterestRate,
    string InstallmentType,
    int DurationInInterestUnits,
    int RepaymentDurations,
    double InstallmentAmount,
    DateOnly? IssuanceDate,
    DateOnly? EndDate,
    DateOnly? NextInstallmentDate,
    string LoanDisbursementMethod,
    string LoanRepaymentMethod,
    string GuarantorsInformation,
    string LoanStatus,
    DateOnly? ClosedDate,
    double TotalAmountPayable,
    double PaidAmount,
    double ProcessingFee,
    double InsuranceAmount
);
