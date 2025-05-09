using System.Security.Claims;

namespace LoanTrack.Web.Shared.Common;

public static class ClaimsPrincipalExtensions
{
    public static bool IsInAnyRole(this ClaimsPrincipal user, params string[] roles)
    {
        return roles.Any(user.IsInRole);
    }
}
