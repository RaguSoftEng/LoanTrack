using System.Net;
using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.Data;
using LoanTrack.Domain.Common;
using LoanTrack.Domain.Groups;

namespace LoanTrack.Application.Groups.Commands.Update;

public class UpdateGroupCommandHandler(
    ICustomerGroupRepository groupRepository,
    IUnitOfWork unitOfWork
): ICommandHandler<UpdateGroupCommand>
{
    public async Task<Result> Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var group = await groupRepository.GetByIdAsync(request.GroupId, cancellationToken);
            if (group == null) return Result.Failure(GroupErrors.NotFound(request.GroupId));
            
            var isExists = await groupRepository.IsExistAsync(
                x => x.CenterId == group.CenterId
                     && x.Id != group.Id
                     && x.Name == request.Name,
                cancellationToken
            );

            if (isExists)
                return Result.Failure<Guid>(Error.Failure("Group.Conflict", $"There is already a group with the name {request.Name}"));
            
            group.Update(request.Name, request.Description, request.CenterId);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Result.Failure(Error.Failure("Group.InternalServerError", "Unable to update group"));
        }
    }
}
