using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.Data;
using LoanTrack.Domain.Common;
using LoanTrack.Domain.Groups;

namespace LoanTrack.Application.Groups.Commands.Create;

public class CreateGroupCommandHandler(
    ICustomerGroupRepository groupRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<CreateGroupCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var isExists = await groupRepository.IsExistAsync(
                x => x.CenterId == request.CenterId
                     && x.Name == request.Name,
                cancellationToken
            );

            if (isExists)
                return Result.Failure<Guid>(Error.Failure("Group.Conflict", $"There is already a group with the name {request.Name} for the selected center!"));
            
            var group = CustomerGroup.Create(
                request.Name,
                request.Description,
                request.CenterId
            );

            await groupRepository.AddAsync(group, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return group.Id;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Result.Failure<Guid>(Error.Failure("500", ex.Message));
        }
    }
}

