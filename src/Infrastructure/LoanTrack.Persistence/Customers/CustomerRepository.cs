using LoanTrack.Domain.Customers;
using LoanTrack.Persistence.Common;
using LoanTrack.Persistence.Common.Database;

namespace LoanTrack.Persistence.Customers;

public class CustomerRepository(ApplicationDbContext context)
    : GenericRepository<Customer>(context), ICustomerRepository;
