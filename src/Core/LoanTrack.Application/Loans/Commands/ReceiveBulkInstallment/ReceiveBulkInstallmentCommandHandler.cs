using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.Data;
using LoanTrack.Domain.Common;
using LoanTrack.Domain.Common.Constants;
using LoanTrack.Domain.Finance;
using LoanTrack.Domain.Loans;

namespace LoanTrack.Application.Loans.Commands.ReceiveBulkInstallment;

public class ReceiveBulkInstallmentCommandHandler(
    ILoanRepository repository,
    ILoanInstallmentRepository installmentRepository,
    IFinanceJournalRepository journalRepository,
    IUnitOfWork unitOfWork
):ICommandHandler<ReceiveBulkInstallmentCommand>
{
    public async Task<Result> Handle(ReceiveBulkInstallmentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            bool isPaidFound = false;
            foreach ((Guid instalmentId, double paidAmount, DateOnly paymentDate, bool isDelayed, int delayedDays) in request.Installments)
            {
                var installment = await installmentRepository.GetByIdAsync(instalmentId, cancellationToken);
                if (installment!.IsPaid)
                {
                    isPaidFound = true;
                    break;
                }
            
                installment!.ReceiveInstalment(
                    paymentDate,
                    paidAmount,
                    installment.PaymentMethod,
                    installment.PaymentDescription,
                    isDelayed,
                    delayedDays,
                    false,
                    0,
                    ""
                );

                var loan = await repository.GetByIdAsync(installment.LoanId, cancellationToken);
                if(loan == null) throw new Exception("Loan not found");
            
                loan.InstallmentPaid(paidAmount);
                
                var journals = installment.CreateFinanceJournals(loan.LoanAmount / loan.RepaymentDurations);
                if (journals.Any())
                {
                    await journalRepository.AddRangeAsync(journals, cancellationToken);
                }

                if (loan.LoanStatus == LoanStatuses.Closed)
                {
                    continue;
                }

                var newInstallment = loan.GenerateInstallments(installment.InstallmentNumber + 1);
                await installmentRepository.AddAsync(newInstallment, cancellationToken);
            }

            if (isPaidFound) return Result.Failure(Error.Failure("500", "Unable to process the Payment. One or more installments already paid."));
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
