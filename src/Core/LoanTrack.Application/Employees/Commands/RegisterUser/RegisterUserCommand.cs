using LoanTrack.Application.Common.CQRS;

namespace LoanTrack.Application.Employees.Commands.RegisterUser;

public sealed record RegisterUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string UserRole
) : ICommand<Guid>;
