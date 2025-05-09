namespace LoanTrack.Application.Employees.Queries.Auth;

public record AuthEmployeeResponse(
    Guid UserId,
    string FirstName,
    string LastName,
    string Email,
    string UserRole,
    string IdentityId
);
