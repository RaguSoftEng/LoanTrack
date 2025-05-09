namespace LoanTrack.Application.Finance.Reports.LoanSummary;

public record LoanSummaryResponse
{
    public Guid LoanId { get; private set; }
    public string LoanNumber { get; private set; }
    public string Customer  { get; private set; }
    public double LoanAmount { get; private set; }
    public double TotalPayable { get; private set; }
    public double CapitalGained { get; private set; }
    public double CapitalBalance { get; private set; }
    public double InterestPaid { get; private set; }
    public double InterestBalance { get; private set; }
    public double Penalty { get; private set; }
    public double ProcessingFee { get; private set; }
    public double Insurance { get; private set; }
    public string LoanStatus { get; private set; }

    public static LoanSummaryResponse Create(
        Guid loanId,
        string loanNumber,
        string customer,
        double loanAmount,
        double totalPayable,
        double capitalGained,
        double interestPaid,
        double penalty,
        double processingFee,
        double insurance,
        string loanStatus
    )
    {
        var obj = new LoanSummaryResponse
        {
            LoanId = loanId,
            LoanNumber = loanNumber,
            Customer = customer,
            LoanAmount = loanAmount,
            TotalPayable = totalPayable,
            CapitalGained = capitalGained,
            CapitalBalance = loanAmount - capitalGained,
            InterestPaid = interestPaid,
            Penalty = penalty,
            ProcessingFee = processingFee,
            Insurance = insurance,
            LoanStatus = loanStatus,
            InterestBalance = totalPayable - loanAmount - interestPaid
        };
        
        return obj;
    }
};
