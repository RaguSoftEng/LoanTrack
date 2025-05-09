namespace LoanTrack.Domain.Common.Constants;

public class FinanceReferenceType
{
    public const string Loan = nameof(Loan);

    private static readonly HashSet<string> Valid = [Loan];

    public static string Validate(string value) =>
        Valid.Contains(value) ? value : throw new ArgumentException("Invalid reference type");
}
