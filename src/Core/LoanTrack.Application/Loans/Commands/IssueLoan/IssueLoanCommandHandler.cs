using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.Data;
using LoanTrack.Domain.Common;
using LoanTrack.Domain.Common.Constants;
using LoanTrack.Domain.Finance;
using LoanTrack.Domain.Loans;

namespace LoanTrack.Application.Loans.Commands.IssueLoan;

public class IssueLoanCommandHandler(
    ILoanRepository repository,
    ILoanInstallmentRepository installmentRepository,
    IFinanceJournalRepository journalRepository,
    IUnitOfWork unitOfWork
): ICommandHandler<IssueLoanCommand>
{
    public async Task<Result> Handle(IssueLoanCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var loan = await repository.GetByIdAsync(request.LoanId, cancellationToken);

            loan!.Issue(request.IssueDate, request.FistInstallmentDate);

            var loanJournal = FinanceJournal.Create(
                request.IssueDate,
                JournalTypes.LoanIssued,
                loan.LoanAmount,
                FinanceReferenceType.Loan,
                loan.Id
            );
            await journalRepository.AddAsync(loanJournal, cancellationToken);

            if (loan.InsuranceAmount > 0)
            {
                var insuranceJournal = FinanceJournal.Create(
                    request.IssueDate,
                    JournalTypes.Insurance,
                    loan.InsuranceAmount,
                    FinanceReferenceType.Loan,
                    loan.Id
                );
                await journalRepository.AddAsync(insuranceJournal, cancellationToken);
            }

            if (loan.ProcessingFee > 0)
            {
                var processingFeeJournal = FinanceJournal.Create(
                    request.IssueDate,
                    JournalTypes.ProcessingFee,
                    loan.ProcessingFee,
                    FinanceReferenceType.Loan,
                    loan.Id
                );
                await journalRepository.AddAsync(processingFeeJournal, cancellationToken);
            }
            
            var installment = loan!.GenerateInstallments(1);
            await installmentRepository.AddAsync(installment, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return  Result.Failure(Error.Failure( "500",ex.Message));
        }
    }
}
