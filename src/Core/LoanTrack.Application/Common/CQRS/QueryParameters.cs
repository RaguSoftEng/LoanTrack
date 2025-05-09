namespace LoanTrack.Application.Common.CQRS;

public record QueryParameters
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string SearchBy { get; init; } = string.Empty;
    public string Search { get; init; } = string.Empty;
    public string? SortBy { get; init; }
    public bool SortDescending { get; init; }
}
