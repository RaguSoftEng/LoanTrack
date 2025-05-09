namespace LoanTrack.Domain.Common.Constants;

public static class InterestTypes
{
    public const string PerDay = "Per-Day";
    public const string PerWeek = "Per-Week";
    public const string PerMonth = "Per-Month";
    public const string PerAnnum = "Per-Annum";
    
    private static readonly HashSet<string> ValidTypes = [PerDay, PerWeek, PerMonth, PerAnnum];
    
    public static string Validate(string value)
        => ValidTypes.Contains(value)
            ? value : throw new ArgumentException("Invalid Interest type");
    
    public static IReadOnlyCollection<string> GetTypes => [.. ValidTypes];
}
