using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.Data;
using LoanTrack.Domain.Common;
using LoanTrack.Domain.ListValues;
using Microsoft.Extensions.Logging;

namespace LoanTrack.Application.ListValues.Commands.BulkUpload;

public class ListValuesBulkUploadCommandHandler(
    IListValueRepository repository,
    IUnitOfWork unitOfWork,
    ILogger<ListValuesBulkUploadCommandHandler> logger
): ICommandHandler<ListValuesBulkUploadCommand>
{
    public async Task<Result> Handle(ListValuesBulkUploadCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var listValues = request.ListValues.Select(x=> ListValue.Create(request.ListType, x, request.ParentId)).ToList();
            await repository.AddRangeAsync(listValues, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception e)
        {
            logger.LogError(e.Message, e);
            return Result.Failure(Error.Failure("500", "There was an error saving your list values, Make sure there is no any duplicate values."));
        } 
    }
}
