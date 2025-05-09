using LoanTrack.Application.Common.CQRS;

namespace LoanTrack.Application.Loans.Commands.CreateLoan;

public record CreateLoanCommand(
    string LoanNumber,
    Guid CustomerId,
    Guid? LoanSchemeId,
    string LoanOfficer,
    string InterestType,
    double LoanAmount,
    double InterestRate,
    string InstallmentType,
    int DurationInInterestUnits,
    int RepaymentDurations,
    double InstallmentAmount,
    DateOnly? IssuanceDate,
    DateOnly? NextInstallmentDate,
    string LoanDisbursementMethod,
    string LoanRepaymentMethod,
    string GuarantorsInformation,
    double TotalAmountPayable,
    double ProcessingFee,
    double InsuranceAmount
) : ICommand<Guid>;
