using LoanTrack.Application.Common.CQRS;

namespace LoanTrack.Application.LoanSchemes.Commands.Update;

public record UpdateLoanSchemeCommand(
    Guid Id,
    string Description,
    string InterestType,
    double InterestRate,
    double MinimumAmount,
    double MaximumAmount,
    int RepaymentPeriodsInMonths,
    double ProcessingFee,
    double InsuranceAmount,
    double LatePaymentPenalty,
    bool IsSecuredLoan,
    string CollateralType,
    bool HasFixedInterestRate,
    bool IsGovernmentSubsidized,
    List<string> EligibleBorrowerTypes,
    List<string> AllowedLoanPurposes,
    bool RequiresGuarantor,
    int GracePeriodInMonths,
    bool IsActive
) : ICommand;
