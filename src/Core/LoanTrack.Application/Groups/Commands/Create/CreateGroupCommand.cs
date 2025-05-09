using LoanTrack.Application.Common.CQRS;

namespace LoanTrack.Application.Groups.Commands.Create;

public record CreateGroupCommand(string Name, string Description, Guid CenterId) : ICommand<Guid>;
