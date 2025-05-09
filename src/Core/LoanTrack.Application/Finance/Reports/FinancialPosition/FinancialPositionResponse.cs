namespace LoanTrack.Application.Finance.Reports.FinancialPosition;

public record FinancialPositionResponse
{
    public DateOnly StartDate { get; init; }
    public DateOnly EndDate { get; init; }
    public double TotalLoanOutstanding { get; init; }
    public double TotalIncome { get; init; }
    public double Liabilities { get; init; }
};
