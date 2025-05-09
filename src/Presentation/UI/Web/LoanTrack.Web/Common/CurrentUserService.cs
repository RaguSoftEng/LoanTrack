using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace LoanTrack.Web.Common;

public class CurrentUserService(AuthenticationStateProvider authProvider)
{
    private ClaimsPrincipal _user;

    public async Task<ClaimsPrincipal> GetUserAsync()
    {
        if (_user != null)
        {
            return _user;
        }

        var authState = await authProvider.GetAuthenticationStateAsync();
        _user = authState.User;
        return _user;
    }

    public async Task<bool> IsInAnyRole(params string[] roles)
    {
        var user = await GetUserAsync();
        return roles.Any(user.IsInRole);
    }
}
