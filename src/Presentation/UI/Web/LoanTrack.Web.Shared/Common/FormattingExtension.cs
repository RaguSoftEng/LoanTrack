using System.Globalization;

namespace LoanTrack.Web.Shared.Common;

public static class FormattingExtension
{
    public static string ToOrdinal(this int number)
    {
        if (number <= 0) return number.ToString(CultureInfo.CurrentCulture);

        int rem100 = number % 100;
        int rem10 = number % 10;

        if (rem100 is >= 11 and <= 13)
            return $"{number}th";

        return rem10 switch
        {
            1 => $"{number}st",
            2 => $"{number}nd",
            3 => $"{number}rd",
            _ => $"{number}th"
        };
    }
}
