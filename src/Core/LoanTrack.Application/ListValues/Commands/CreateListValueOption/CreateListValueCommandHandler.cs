using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.Data;
using LoanTrack.Domain.Common;
using LoanTrack.Domain.ListValues;

namespace LoanTrack.Application.ListValues.Commands.CreateListValueOption;

public class CreateListValueCommandHandler(
    IListValueRepository listValueRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<CreateListValueCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateListValueCommand request, CancellationToken cancellationToken)
    {
        try
        {

            var isExists = await listValueRepository.IsExistAsync(
                x => x.ParentId == request.ParentId
                     && x.Description == request.Description,
                cancellationToken
            );

            if (isExists)
                return Result.Failure<Guid>(Error.Failure("500", $"The value you are trying to add already exists"));
            
            var listValue = ListValue.Create(request.ListType, request.Description, request.ParentId);
        
            await listValueRepository.AddAsync(listValue, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        
            return listValue.Id;
        }
        catch (Exception ex)
        {
            return Result.Failure<Guid>(Error.Failure("500", ex.Message));
        }
    }
}
