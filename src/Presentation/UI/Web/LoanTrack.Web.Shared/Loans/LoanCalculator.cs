using LoanTrack.Domain.Common.Constants;

namespace LoanTrack.Web.Shared.Loans;

public static class LoanCalculator
{

    public static (double TotalPayable, double Installment) CalculateInstallment(
        double loanAmount,
        double interestRatePerUnit,
        int durationInInterestUnit,
        int duration
    )
    {
        var totalInterest = loanAmount * (interestRatePerUnit / 100) * durationInInterestUnit;
        var totalPayable = loanAmount + totalInterest;
        return (totalPayable, totalPayable / duration);
    }
    
    public static int CalDuration(string interestType, string installmentType, int durationInInterestUnits)
    {
        if (durationInInterestUnits <= 0)
        {
            return 0;
        }

        return interestType switch
        {
            InterestTypes.PerAnnum => installmentType switch
            {
                InstallmentTypes.Monthly => durationInInterestUnits * 12,
                InstallmentTypes.Weekly => durationInInterestUnits * 52,
                InstallmentTypes.Daily => durationInInterestUnits * 365,
                _ => durationInInterestUnits
            },
            InterestTypes.PerMonth => installmentType switch
            {
                InstallmentTypes.Weekly => durationInInterestUnits * 4,
                InstallmentTypes.Daily => durationInInterestUnits * 30,
                _ => durationInInterestUnits
            },
            InterestTypes.PerWeek => installmentType switch
            {
                InstallmentTypes.Daily => durationInInterestUnits * 7,
                _ => durationInInterestUnits
            },
            _ => durationInInterestUnits
        };
    }
}
