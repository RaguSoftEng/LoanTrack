using LoanTrack.Domain.Common;
using LoanTrack.Domain.Common.Constants;

namespace LoanTrack.Domain.Finance;

public class FinanceJournal : AuditableEntity
{
    public DateOnly JournalDate { get; private set; }
    public string JournalType { get; private set; }
    public double Amount { get; private set; }
    public string ReferenceType { get; private set; }
    public Guid ReferenceId { get; private set; }


    public static FinanceJournal Create(
        DateOnly journalDate,
        string journalType,
        double amount,
        string referenceType,
        Guid referenceId
    ) => new()
    {
        JournalDate = journalDate,
        JournalType = JournalTypes.Validate(journalType),
        Amount = amount,
        ReferenceType = FinanceReferenceType.Validate(referenceType),
        ReferenceId = referenceId
    };
}
