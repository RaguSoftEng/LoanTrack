using LoanTrack.Domain.Common;
using LoanTrack.Domain.Common.Constants;

namespace LoanTrack.Domain.LoanSchemes;

public sealed class LoanScheme : AuditableEntity
{
    public string Code { get; set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string InterestType { get; private set; }
    public double InterestRate { get; private set; }
    public double MinimumAmount { get; private set; }
    public double MaximumAmount { get; private set; }
    public int RepaymentPeriodsInMonths { get; private set; }
    public double ProcessingFee { get; private set; }
    public double InsuranceAmount { get; private set; }
    public double LatePaymentPenalty { get; private set; }
    public bool IsSecuredLoan { get; private set; }
    public string? CollateralType { get; private set; }
    public bool HasFixedInterestRate { get; private set; }
    public bool IsGovernmentSubsidized { get; private set; }
    public List<string> EligibleBorrowerTypes { get; private set; } = [];
    public List<string> AllowedLoanPurposes { get; private set; } = [];
    public bool RequiresGuarantor { get; private set; }
    public int GracePeriodInMonths { get; private set; }
    public bool IsActive { get; private set; }

    private LoanScheme() { }

    public static LoanScheme Create(
        string code,
        string name,
        string description,
        string interestType,
        double interestRate,
        double minimumAmount,
        double maximumAmount,
        int repaymentPeriodsInMonths,
        double processingFee,
        double insuranceAmount,
        double latePaymentPenalty,
        bool isSecuredLoan,
        string? collateralType,
        bool hasFixedInterestRate,
        bool isGovernmentSubsidized,
        List<string> eligibleBorrowerTypes,
        List<string> allowedLoanPurposes,
        bool requiresGuarantor,
        int gracePeriodInMonths,
        bool isActive
    ) => new()
    {
        Code = code,
        Name = name,
        Description = description,
        InterestType = InterestTypes.Validate(interestType),
        InterestRate = interestRate,
        MinimumAmount = minimumAmount,
        MaximumAmount = maximumAmount,
        RepaymentPeriodsInMonths = repaymentPeriodsInMonths,
        ProcessingFee = processingFee,
        InsuranceAmount = insuranceAmount,
        LatePaymentPenalty = latePaymentPenalty,
        IsSecuredLoan = isSecuredLoan,
        CollateralType = collateralType,
        HasFixedInterestRate = hasFixedInterestRate,
        IsGovernmentSubsidized = isGovernmentSubsidized,
        EligibleBorrowerTypes = eligibleBorrowerTypes,
        AllowedLoanPurposes = allowedLoanPurposes,
        RequiresGuarantor = requiresGuarantor,
        GracePeriodInMonths = gracePeriodInMonths,
        IsActive = isActive
    };

    public void Update(
        string description,
        string interestType,
        double interestRate,
        double minimumAmount,
        double maximumAmount,
        int repaymentPeriodsInMonths,
        double processingFee,
        double insuranceAmount,
        double latePaymentPenalty,
        bool isSecuredLoan,
        string? collateralType,
        bool hasFixedInterestRate,
        bool isGovernmentSubsidized,
        List<string> eligibleBorrowerTypes,
        List<string> allowedLoanPurposes,
        bool requiresGuarantor,
        int gracePeriodInMonths,
        bool isActive
    )
    {
        Description = description;
        InterestType = interestType;
        InterestRate = interestRate;
        MinimumAmount = minimumAmount;
        MaximumAmount = maximumAmount;
        RepaymentPeriodsInMonths = repaymentPeriodsInMonths;
        ProcessingFee = processingFee;
        InsuranceAmount = insuranceAmount;
        LatePaymentPenalty = latePaymentPenalty;
        IsSecuredLoan = isSecuredLoan;
        CollateralType = collateralType;
        HasFixedInterestRate = hasFixedInterestRate;
        IsGovernmentSubsidized = isGovernmentSubsidized;
        EligibleBorrowerTypes = [.. eligibleBorrowerTypes];
        AllowedLoanPurposes = [.. allowedLoanPurposes];
        RequiresGuarantor = requiresGuarantor;
        GracePeriodInMonths = gracePeriodInMonths;
        IsActive = isActive;
    }
}
 
