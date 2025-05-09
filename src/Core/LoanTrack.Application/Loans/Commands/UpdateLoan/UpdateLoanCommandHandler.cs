using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.Data;
using LoanTrack.Domain.Common;
using LoanTrack.Domain.Common.Constants;
using LoanTrack.Domain.Loans;

namespace LoanTrack.Application.Loans.Commands.UpdateLoan;

public class UpdateLoanCommandHandler(
    ILoanRepository repository,
    IUnitOfWork unitOfWork
) : ICommandHandler<UpdateLoanCommand>
{
    public async Task<Result> Handle(UpdateLoanCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var loan = await repository.GetByIdAsync(request.LoanId, cancellationToken);
            if(loan == null) return Result.Failure(Error.Failure("404", "Loan Not Found"));
            if(loan.LoanStatus != LoanStatuses.Pending) return Result.Failure(Error.Failure("401", "Loan cannot be updated"));
            
            loan.Update(
                request.LoanSchemeId,
                request.LoanOfficer,
                request.LoanAmount,
                request.InterestType,
                request.InterestRate,
                request.InstallmentType,
                request.DurationInInterestUnits,
                request.RepaymentDurations,
                request.InstallmentAmount,
                request.IssuanceDate,
                request.NextInstallmentDate,
                request.LoanDisbursementMethod,
                request.LoanRepaymentMethod,
                request.GuarantorsInformation,
                request.TotalAmountPayable,
                request.ProcessingFee,
                request.InsuranceAmount
            );
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Result.Failure(Error.Failure("404", "Loan update failed"));
        }
    }
}
