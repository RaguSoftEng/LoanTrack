using LoanTrack.Application.Common.CQRS;

namespace LoanTrack.Application.Centers.Commands.Update;

public record UpdateCenterCommand(Guid Id,string Name, string Description, Guid? CenterLeaderId) : ICommand;
