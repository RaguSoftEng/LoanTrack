using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.Data;
using LoanTrack.Domain.Common;
using LoanTrack.Domain.Common.Constants;
using LoanTrack.Domain.Finance;
using LoanTrack.Domain.Loans;

namespace LoanTrack.Application.Loans.Commands.ReceiveInstalment;

public class ReceiveInstallmentCommandHandler(
    ILoanInstallmentRepository installmentRepository,
    ILoanRepository loanRepository,
    IFinanceJournalRepository journalRepository,
    IUnitOfWork unitOfWork
): ICommandHandler<ReceiveInstallmentCommand>
{
    public async Task<Result> Handle(ReceiveInstallmentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var installment = await installmentRepository.GetByIdAsync(request.InstalmentId, cancellationToken);
            if(installment!.IsPaid) return Result.Failure(Error.Failure("400", "Installment already paid"));
            
            installment!.ReceiveInstalment(
                request.PaymentDate,
                request.Amount,
                request.PaymentMethod,
                request.PaymentDescription,
                request.IsDelayed,
                request.DaysDelayed,
                request.IsPenaltyApplied,
                request.PenaltyAmount,
                request.PenaltyReason
            );

            var loan = await loanRepository.GetByIdAsync(installment.LoanId, cancellationToken);
            if(loan == null) throw new Exception("Loan not found");
            
            loan.InstallmentPaid(request.Amount-request.PenaltyAmount);

            if (loan.LoanStatus != LoanStatuses.Closed)
            {
                var newInstallment = loan.GenerateInstallments(installment.InstallmentNumber + 1);
                await installmentRepository.AddAsync(newInstallment, cancellationToken);
            }

            var journals = installment.CreateFinanceJournals(loan.LoanAmount / loan.RepaymentDurations);
            if (journals.Any())
            {
                await journalRepository.AddRangeAsync(journals, cancellationToken);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Result.Failure(Error.Failure("500", "Unable to process the Payment"));
        }
    }
}
