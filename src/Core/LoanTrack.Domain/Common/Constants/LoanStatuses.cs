namespace LoanTrack.Domain.Common.Constants;

public static class LoanStatuses
{
    public const string Pending = "Pending";
    public const string Approved = "Approved";
    public const string Rejected = "Rejected";
    public const string CanceledByCustomer = "CanceledByCustomer";
    public const string Issued = "Issued";
    public const string Ongoing = "Ongoing";
    public const string Closed = "Closed";
    
    private static readonly HashSet<string> ValidStatus = [Pending, Approved, Rejected, CanceledByCustomer, Issued, Ongoing, Closed];
    
    public static string Validate(string value)
        => ValidStatus.Contains(value)
            ? value : throw new ArgumentException("Invalid Repayment Method");
    
    public static IReadOnlyCollection<string> GetStatus => [.. ValidStatus];
}
