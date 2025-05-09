namespace LoanTrack.Domain.Common.Constants;

public static class JournalTypes
{
    public const string LoanIssued = nameof(LoanIssued);
    public const string LoanRepayment = nameof(LoanRepayment);
    public const string InterestIncome = nameof(InterestIncome);
    public const string ProcessingFee = nameof(ProcessingFee);
    public const string PenaltyIncome = nameof(PenaltyIncome);
    public const string Insurance = nameof(Insurance);

    private static readonly HashSet<string> ValidTypes = [LoanIssued, LoanRepayment, InterestIncome, ProcessingFee, PenaltyIncome, Insurance];
    
    public static string Validate(string value)
        => ValidTypes.Contains(value)
            ? value : throw new ArgumentException("Invalid transaction type");
    
    public static IReadOnlyCollection<string> GetTypes => [.. ValidTypes];
    public static IReadOnlyCollection<string> CollectionTypes => [LoanRepayment, InterestIncome, PenaltyIncome];
    public static IReadOnlyCollection<string> ProfitTypes => [InterestIncome, PenaltyIncome, ProcessingFee];
    
    public static bool IsIncome(string type) => type is InterestIncome or ProcessingFee or PenaltyIncome or Insurance;

    public static bool IsExpense(string type) => type == LoanIssued;

    public static bool IsCashIn(string type) =>
        type is LoanRepayment or InterestIncome or ProcessingFee or PenaltyIncome or Insurance;

}
