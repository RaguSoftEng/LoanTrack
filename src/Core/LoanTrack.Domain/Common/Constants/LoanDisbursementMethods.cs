namespace LoanTrack.Domain.Common.Constants;

public static class LoanDisbursementMethods
{
    public const string CashOnHand = "CashOnHand";
    public const string BankDeposit = "BankDeposit";
    public const string Cheque = "Cheque";
    
    private static readonly HashSet<string> ValidMethods = [CashOnHand, BankDeposit, Cheque];
    
    public static string Validate(string value)
        => ValidMethods.Contains(value)
            ? value : throw new ArgumentException("Invalid Disbursement Method");
    
    public static IReadOnlyCollection<string> GetMethods => [.. ValidMethods];
}
