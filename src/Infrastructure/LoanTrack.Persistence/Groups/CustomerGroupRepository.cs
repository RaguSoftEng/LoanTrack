using LoanTrack.Domain.Groups;
using LoanTrack.Persistence.Common;
using LoanTrack.Persistence.Common.Database;

namespace LoanTrack.Persistence.Groups;

public class CustomerGroupRepository(ApplicationDbContext context)
    : GenericRepository<CustomerGroup>(context), ICustomerGroupRepository;
