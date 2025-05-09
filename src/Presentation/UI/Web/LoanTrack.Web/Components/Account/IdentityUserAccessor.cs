using LoanTrack.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace LoanTrack.Web.Components.Account;

internal sealed class IdentityUserAccessor(
    UserManager<AppIdentityUser> userManager,
    IdentityRedirectManager redirectManager)
{
    public async Task<AppIdentityUser> GetRequiredUserAsync(HttpContext context)
    {
        var user = await userManager.GetUserAsync(context.User);

        if (user is null)
        {
            redirectManager.RedirectToWithStatus("Account/InvalidUser",
                $"Error: Unable to load user with ID '{userManager.GetUserId(context.User)}'.", context);
        }

        return user;
    }
}
