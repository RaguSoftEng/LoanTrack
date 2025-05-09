using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.Data;
using LoanTrack.Domain.Centers;
using LoanTrack.Domain.Common;

namespace LoanTrack.Application.Centers.Commands.Update;

public class UpdateCenterCommandHandler(
    ICenterRepository repository,
    IUnitOfWork unitOfWork
): ICommandHandler<UpdateCenterCommand>
{
    public async Task<Result> Handle(UpdateCenterCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var center = await repository.GetByIdAsync(request.Id, cancellationToken);
            if(center == null) return Result.Failure(Error.NotFound("404", "Center not found"));
            
            var isExists = await repository.IsExistAsync(
                x => x.Name == request.Name,
                cancellationToken
            );

            if (isExists)
                return Result.Failure<Guid>(Error.Failure("500", $"There is already a center with name {request.Name}"));
            
            center.Update(
                request.Name,
                request.Description,
                request.CenterLeaderId == Guid.Empty ? null : request.CenterLeaderId
            );
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Result.Failure(Error.Failure("500", "Unable to update center"));
        }
    }
}
