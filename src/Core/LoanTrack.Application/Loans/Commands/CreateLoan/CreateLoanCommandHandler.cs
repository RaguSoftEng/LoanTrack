using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.Data;
using LoanTrack.Domain.Common;
using LoanTrack.Domain.Loans;

namespace LoanTrack.Application.Loans.Commands.CreateLoan;

public class CreateLoanCommandHandler(
    ILoanRepository repository,
    IUnitOfWork unitOfWork
) : ICommandHandler<CreateLoanCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateLoanCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var isExists = await repository.IsExistAsync(
                x => x.LoanNumber == request.LoanNumber,
                cancellationToken
            );

            if (isExists)
                return Result.Failure<Guid>(Error.Failure("500", $"There is already a loan with the number {request.LoanNumber}"));
            
            var loan = Loan.Create(
                request.LoanNumber,
                request.CustomerId,
                request.LoanSchemeId,
                request.LoanOfficer,
                request.InterestType,
                request.LoanAmount,
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
        
            await repository.AddAsync(loan, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return loan.Id;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Result.Failure<Guid>(Error.Failure("500", ex.Message));
        }
    }
}
