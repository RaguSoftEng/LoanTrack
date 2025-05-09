using LoanTrack.Application.Common.CQRS;

namespace LoanTrack.Application.ListValues.Commands.Update;

public record ListValueUpdateCommand(Guid Id, string Value) : ICommand;
