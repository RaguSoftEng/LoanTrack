using LoanTrack.Domain.Loans;
using LoanTrack.Persistence.Common;
using LoanTrack.Persistence.Common.Database;

namespace LoanTrack.Persistence.Loans;

public class LoanInstallmentRepository(ApplicationDbContext context)
    : GenericRepository<LoanInstallment>(context), ILoanInstallmentRepository;
