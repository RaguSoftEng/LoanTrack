namespace LoanTrack.Application.Employees.Queries.GetUsers;

public record GetUsersResponse(
    Guid UserId,
    string FirstName,
    string LastName,
    string Email,
    string UserRole,
    bool IsActive
);
