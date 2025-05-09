using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.Data;
using LoanTrack.Domain.Centers;
using LoanTrack.Domain.Common;

namespace LoanTrack.Application.Centers.Commands.Create;

public class CreatCenterCommandHandler(
    ICenterRepository repository,
    IUnitOfWork unitOfWork
) : ICommandHandler<CreateCenterCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateCenterCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var isExists = await repository.IsExistAsync(
                x => x.Name == request.Name,
                cancellationToken
            );

            if (isExists)
                return Result.Failure<Guid>(Error.Failure("500", $"There is already a center with name {request.Name}"));
            
            var center = Center.Create(request.Name, request.Description, request.CenterLeaderId);
        
            await repository.AddAsync(center, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return center.Id;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Result.Failure<Guid>(Error.Failure("500", ex.Message));
        }
    }
}
