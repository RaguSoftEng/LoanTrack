using LoanTrack.Domain.Common;

namespace LoanTrack.Application.Common.Identity;

public static class IdentityProviderErrors
{
    public static readonly Error EmailIsNotUnique = Error.Conflict(
        "Identity.EmailIsNotUnique",
        "The specified email is not unique."
    );   
}
