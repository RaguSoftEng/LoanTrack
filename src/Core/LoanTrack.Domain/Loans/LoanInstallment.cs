using LoanTrack.Domain.Common;
using LoanTrack.Domain.Common.Constants;
using LoanTrack.Domain.Common.Extensions;
using LoanTrack.Domain.Finance;

namespace LoanTrack.Domain.Loans;

public class LoanInstallment : BaseEntity
{
    public Guid LoanId { get; private init; }
    public Loan Loan { get; private set; }
    public int InstallmentNumber { get; private set; }
    public DateOnly InstallmentDate { get; private set; }
    public double InstallmentAmount { get; private init; }
    public bool IsPaid { get; private set; }
    public bool IsDelayed { get; private set; }
    public int DelayedDays { get; private set; }
    public bool IsPenaltyApplied { get; private set; }
    public double PenaltyAmount { get; private set; }
    public string PenaltyReason { get; private set; }
    public DateOnly? PaymentDate { get; private set; }
    public double AmountPaid { get; private set; }
    public string PaymentMethod { get; private set; }
    public string PaymentDescription { get; private set; }
    
    private LoanInstallment() { }
    
    public static LoanInstallment Create(
        Guid loanId,
        int installmentNumber,
        DateOnly installmentDate,
        double installmentAmount
    )
    {
        var installment = new LoanInstallment
        {
            LoanId = loanId,
            InstallmentNumber = installmentNumber,
            InstallmentDate = installmentDate,
            InstallmentAmount = installmentAmount,
        };

        return installment;
    }

    public void ReceiveInstalment(
        DateOnly paymentDate,
        double amountPaid,
        string paymentMethod,
        string paymentDescription,
        bool isDelayed,
        int delayedDays,
        bool isPenaltyApplied,
        double penaltyAmount,
        string penaltyReason
    )
    {
        IsPaid = true;
        PaymentDate = paymentDate.IsValid() ? paymentDate : DateOnly.FromDateTime(DateTime.UtcNow);
        AmountPaid = amountPaid;
        PaymentMethod = paymentMethod;
        PaymentDescription = paymentDescription;
        IsDelayed = isDelayed;
        DelayedDays = delayedDays;
        IsPenaltyApplied = isPenaltyApplied;
        PenaltyAmount = penaltyAmount;
        PenaltyReason = penaltyReason;
    }

    public static (bool IsDelayed, int DaysDelayed) IsPaymentDelayed(DateOnly installmentDate, bool isPaid)
    {
        if (isPaid) return (false, 0);

        int daysDelayed = DateOnly.FromDateTime(DateTime.UtcNow.Date).DayNumber - installmentDate.DayNumber;
        bool isDelayed = daysDelayed > 0;

        return (isDelayed, isDelayed ? daysDelayed : 0);

    }

    public List<FinanceJournal> CreateFinanceJournals(double capitalDue)
    {
        var journals = new List<FinanceJournal>();
        if (!IsPaid) return journals;

        double paidAmount = AmountPaid - PenaltyAmount;
        double capitalPaid;
        double interestPaid;

        if (paidAmount <= InstallmentAmount)
        {
            capitalPaid = capitalDue;
            interestPaid = paidAmount - capitalDue;
        }
        else if (paidAmount < 2 * InstallmentAmount)
        {
            capitalPaid = capitalDue;
            interestPaid = paidAmount - capitalPaid;
        }
        else
        {
            var fullInstallmentCount = (int)(paidAmount / InstallmentAmount);
            capitalPaid = capitalDue * fullInstallmentCount;
            interestPaid = paidAmount - capitalPaid;
        }

        if (capitalPaid > 0)
        {
            journals.Add(FinanceJournal.Create(
                PaymentDate!.Value,
                JournalTypes.LoanRepayment,
                capitalPaid,
                FinanceReferenceType.Loan,
                LoanId
            ));
        }

        if (interestPaid > 0)
        {
            journals.Add(FinanceJournal.Create(
                PaymentDate!.Value,
                JournalTypes.InterestIncome,
                interestPaid,
                FinanceReferenceType.Loan,
                LoanId
            ));
        }

        if (PenaltyAmount > 0)
        {
            journals.Add(FinanceJournal.Create(
                PaymentDate!.Value,
                JournalTypes.PenaltyIncome,
                PenaltyAmount,
                FinanceReferenceType.Loan,
                LoanId
            ));
        }

        return journals;
    }
}
