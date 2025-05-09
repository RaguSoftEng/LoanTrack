namespace LoanTrack.Domain.Common.Constants;

public static class InstallmentTypes
{
    public const string Daily = "Daily";
    public const string Weekly = "Weekly";
    public const string Monthly = "Monthly";
    public const string Yearly = "Yearly";
    public const string SpeedDraft  = "Speed-Draft";
    
    private static readonly HashSet<string> ValidTypes = [Daily, Weekly, Monthly, Yearly, SpeedDraft];
    
    public static string Validate(string value)
        => ValidTypes.Contains(value)
            ? value : throw new ArgumentException("Invalid Repayment Method");
    
    public static IReadOnlyCollection<string> GetTypes => [.. ValidTypes];
}
