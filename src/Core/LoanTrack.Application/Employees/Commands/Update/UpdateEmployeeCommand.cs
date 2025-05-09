
using LoanTrack.Application.Common.CQRS;

namespace LoanTrack.Application.Employees.Commands.Update;

public record UpdateEmployeeCommand(Guid Id, string FirstName, string LastName, string UserRole) : ICommand;
