using LoanTrack.Application.Common.CQRS;

namespace LoanTrack.Application.Employees.Commands.Updateprofile;

public record UpdateProfileCommand(Guid Id, string FirstName, string LastName): ICommand;
