using Microsoft.AspNetCore.Identity;

namespace LoanTrack.Identity.Models;

public class AppIdentityUser : IdentityUser
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
}
