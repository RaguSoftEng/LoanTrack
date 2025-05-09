namespace LoanTrack.Application.Common.CQRS;

public static class QueryParameterExtensions
{
    public static QueryParameters ToBaseQuery(this QueryParameters derived)
    {
        return new QueryParameters
        {
            Page = derived.Page,
            PageSize = derived.PageSize,
            Search = derived.Search,
            SearchBy = derived.SearchBy,
            SortBy = derived.SortBy,
            SortDescending = derived.SortDescending
        };
    }  
}
