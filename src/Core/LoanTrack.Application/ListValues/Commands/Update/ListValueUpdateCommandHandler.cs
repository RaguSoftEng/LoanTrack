using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.Data;
using LoanTrack.Domain.Common;
using LoanTrack.Domain.ListValues;

namespace LoanTrack.Application.ListValues.Commands.Update;

public class ListValueUpdateCommandHandler(
    IListValueRepository repository,
    IUnitOfWork unitOfWork
): ICommandHandler<ListValueUpdateCommand>
{
    public async Task<Result> Handle(ListValueUpdateCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await repository.GetByIdAsync(request.Id, cancellationToken);
            if(entity == null) return Result.Failure(Error.NotFound("400", $"Value Not found!"));
            
            var isExists = await repository.IsExistAsync(
                x => x.ParentId == entity.ParentId
                     && x.Id != entity.Id
                     && x.Description == request.Value,
                cancellationToken
            );
            
            if (isExists)
                return Result.Failure<Guid>(Error.Failure("500", $"The value you are trying to update is already exists!"));
            
            entity.Update(request.Value);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Result.Failure(Error.Failure("500", "Unable to update the list value"));
        }
    }
}
