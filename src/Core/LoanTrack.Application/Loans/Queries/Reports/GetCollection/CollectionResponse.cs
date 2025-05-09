namespace LoanTrack.Application.Loans.Queries.Reports.GetCollection;

public record CollectionResponse
{
    public DateOnly StartDate { get; init; }
    public DateOnly EndDate { get; init; }
    public double TotalCollection { get; init; }
    public IReadOnlyList<(string Center, string Group, double Collection)> GroupCollection { get; init; }
    public IReadOnlyList<(string Center, double Collection)> CenterCollection { get; init; }
};
