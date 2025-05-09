using LoanTrack.Domain.Finance;
using LoanTrack.Persistence.Common;
using LoanTrack.Persistence.Common.Database;

namespace LoanTrack.Persistence.Finance;

public class FinanceJournalRepository(ApplicationDbContext context)
    : GenericRepository<FinanceJournal>(context), IFinanceJournalRepository;
