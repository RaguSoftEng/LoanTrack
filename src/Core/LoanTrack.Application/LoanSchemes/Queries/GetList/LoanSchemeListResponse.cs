namespace LoanTrack.Application.LoanSchemes.Queries.GetList;

public record LoanSchemeListResponse(
    Guid Id,
    string Code,
    string Name,
    double InterestRate,
    double MinimumAmount,
    double MaximumAmount,
    int RepaymentPeriodsInMonths,
    bool HasFixedInterestRate,
    bool RequiresGuarantor,
    int GracePeriodInMonths,
    bool IsActive
);
