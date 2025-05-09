namespace LoanTrack.Application.Common.Identity;

public sealed record UserModel(string Email, string Password, string FirstName, string LastName, string UserRole);
