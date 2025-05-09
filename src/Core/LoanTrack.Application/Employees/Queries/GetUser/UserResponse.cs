namespace LoanTrack.Application.Employees.Queries.GetUser;

public sealed record UserResponse(
    Guid UserId,
    string FirstName,
    string LastName,
    string Email,
    string UserRole,
    bool IsActive
);
