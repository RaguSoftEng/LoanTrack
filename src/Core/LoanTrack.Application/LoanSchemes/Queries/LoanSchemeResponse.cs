using LoanTrack.Domain.LoanSchemes;

namespace LoanTrack.Application.LoanSchemes.Queries;

public record LoanSchemeResponse(
    Guid Id,
    string Code,
    string Name,
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
)
{
    public static LoanSchemeResponse FromEntity(LoanScheme loanScheme) =>
        new(
            loanScheme.Id,
            loanScheme.Code,
            loanScheme.Name,
            loanScheme.Description,
            loanScheme.InterestType,
            loanScheme.InterestRate,
            loanScheme.MinimumAmount,
            loanScheme.MaximumAmount,
            loanScheme.RepaymentPeriodsInMonths,
            loanScheme.ProcessingFee,
            loanScheme.InsuranceAmount,
            loanScheme.LatePaymentPenalty,
            loanScheme.IsSecuredLoan,
            loanScheme.CollateralType ?? "",
            loanScheme.HasFixedInterestRate,
            loanScheme.IsGovernmentSubsidized,
            loanScheme.EligibleBorrowerTypes,
            loanScheme.AllowedLoanPurposes,
            loanScheme.RequiresGuarantor,
            loanScheme.GracePeriodInMonths,
            loanScheme.IsActive
        );
}
