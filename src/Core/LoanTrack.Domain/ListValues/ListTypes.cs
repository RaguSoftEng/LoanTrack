namespace LoanTrack.Domain.ListValues;

public static class ListTypes
{
    public const string PROVINCES = "Provinces";
    public const string DISTRICTS = "Districts";
    public const string DsDivisions = "DsDivisions";
    public const string GRAMANILADHARI = "GramaNiladhari";
    
    private static readonly HashSet<string> ValidListTypes = [PROVINCES, DISTRICTS, DsDivisions, GRAMANILADHARI ];
    
    public static string Validate(string value)
        => ValidListTypes.Contains(value)
            ? value : throw new ArgumentException("Invalid List type");
    
    public static IReadOnlyCollection<string> GetListTypes => [.. ValidListTypes];
}
