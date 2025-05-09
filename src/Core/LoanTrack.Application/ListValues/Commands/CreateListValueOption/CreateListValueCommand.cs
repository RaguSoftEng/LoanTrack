using LoanTrack.Application.Common.CQRS;

namespace LoanTrack.Application.ListValues.Commands.CreateListValueOption;

public record CreateListValueCommand(string ListType, string Description, Guid ParentId) : ICommand<Guid>;
