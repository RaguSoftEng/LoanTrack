namespace LoanTrack.Domain.Common.Constants;

public static class LoanRepaymentMethods
{
    public const string CashOnHand = "CashOnHand";
    public const string BankDeposit = "BankDeposit";
    public const string Cheque = "Cheque";
    
    private static readonly HashSet<string> ValidMethods = [CashOnHand, BankDeposit, Cheque];
    
    public static string Validate(string value)
        => ValidMethods.Contains(value)
            ? value : throw new ArgumentOutOfRangeException(nameof(value), $"'{value}' is not a valid method.");
    
    public static IReadOnlyCollection<string> GetMethods => ValidMethods.ToList().AsReadOnly();
}
