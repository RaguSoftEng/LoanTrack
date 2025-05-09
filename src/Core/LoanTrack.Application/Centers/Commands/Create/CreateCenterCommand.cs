using LoanTrack.Application.Common.CQRS;

namespace LoanTrack.Application.Centers.Commands.Create;

public record CreateCenterCommand(string Name, string Description, Guid? CenterLeaderId) : ICommand<Guid>;
