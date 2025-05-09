using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.Data;
using LoanTrack.Domain.Common;
using LoanTrack.Domain.Common.Constants;
using LoanTrack.Domain.Loans;

namespace LoanTrack.Application.Loans.Commands.UpdateLoanStatus;

public class UpdateLoanStatusCommandHandler(
    ILoanRepository repository,
    IUnitOfWork unitOfWork
) : ICommandHandler<UpdateLoanStatusCommand>
{
    public async Task<Result> Handle(UpdateLoanStatusCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var loan = await repository.GetByIdAsync(request.LoanId, cancellationToken);
            switch (request.Status)
            {
                case LoanStatuses.Approved:
                    {
                        loan!.Approve();
                        await unitOfWork.SaveChangesAsync(cancellationToken);
                        return Result.Success();
                    }
                case LoanStatuses.Rejected:
                    {
                        loan!.Reject();
                        await unitOfWork.SaveChangesAsync(cancellationToken);
                        return Result.Success();
                    }
                case LoanStatuses.CanceledByCustomer:
                    {
                        loan!.CanceledByCustomer();
                        await unitOfWork.SaveChangesAsync(cancellationToken);
                        return Result.Success();
                    }
                case LoanStatuses.Closed:
                    {
                        loan!.Close(request.ActionDate);
                        await unitOfWork.SaveChangesAsync(cancellationToken);
                        return Result.Success();
                    }
                default:
                    throw new Exception($"Unknown loan status: {request.Status}");
            }
        }
        catch (Exception ex)
        {
          return  Result.Failure(Error.Failure( "500",ex.Message));
        }
    }
}
