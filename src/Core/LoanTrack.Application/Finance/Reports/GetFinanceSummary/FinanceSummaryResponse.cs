namespace LoanTrack.Application.Finance.Reports.GetFinanceSummary;

public record FinanceSummaryResponse
{
    public double TotalLoanIssued { get; init; }
    public double TotalCollection { get; init; }
    public double TotalInsurance { get; init; }
    public double TotalProfit { get; init; }
    public double TotalCapitalGain { get; init; }
    public double TotalProcessFee { get; init; }
    public double TotalPenalty { get; init; }
    public double TotalInterestIncome { get; init; }
};
