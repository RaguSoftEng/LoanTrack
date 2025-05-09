using LoanTrack.Application.Common.CQRS;

namespace LoanTrack.Application.Groups.Commands.Update;

public record UpdateGroupCommand(Guid GroupId, string Name, string Description, Guid CenterId) : ICommand;
