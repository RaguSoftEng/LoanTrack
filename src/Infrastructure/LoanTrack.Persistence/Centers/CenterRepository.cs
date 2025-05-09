using LoanTrack.Domain.Centers;
using LoanTrack.Persistence.Common;
using LoanTrack.Persistence.Common.Database;

namespace LoanTrack.Persistence.Centers;

public class CenterRepository(ApplicationDbContext context)
    : GenericRepository<Center>(context), ICenterRepository;

