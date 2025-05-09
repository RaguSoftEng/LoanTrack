using LoanTrack.Domain.Common.Constants;

namespace LoanTrack.Web.Shared.LoanSchemes;

public class LoanSchemeVm
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string InterestType { get; set; }
    public double InterestRate { get; set; }
    
    public double MinimumAmount { get; set; }
    public double MaximumAmount { get; set; }
    public int RepaymentPeriodsInMonths { get; set; }
    public double ProcessingFee { get; set; }
    public double InsuranceAmount { get; set; }
    public double LatePaymentPenalty { get; set; }
    public bool IsSecuredLoan { get; set; }
    public string CollateralType { get; set; }
    public bool HasFixedInterestRate { get; set; }
    public bool IsGovernmentSubsidized { get; set; }
    public List<string> EligibleBorrowerTypes { get; set; } = [];
    public List<string> AllowedLoanPurposes { get; set; } = [];
    public bool RequiresGuarantor { get; set; }
    public int GracePeriodInMonths { get; set; }
    public bool IsActive { get; set; }
}
