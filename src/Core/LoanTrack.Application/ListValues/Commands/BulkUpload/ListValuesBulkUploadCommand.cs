
using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.ListValues.Commands.CreateListValueOption;

namespace LoanTrack.Application.ListValues.Commands.BulkUpload;

public record ListValuesBulkUploadCommand(string ListType, Guid ParentId, List<string> ListValues) : ICommand;
