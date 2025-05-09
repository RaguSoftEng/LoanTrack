namespace LoanTrack.Domain.Common.Extensions;

public static class DateOnlyExtensions
{
    public static bool IsValid(this DateOnly date)
        => date != DateOnly.MinValue && date != DateOnly.MaxValue;
    
    public static bool IsValid(this DateOnly? date)
        => date is not null && date != DateOnly.MinValue && date != DateOnly.MaxValue;
}
