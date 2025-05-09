using LoanTrack.Domain.Employees;
using LoanTrack.Persistence.Common;
using LoanTrack.Persistence.Common.Database;

namespace LoanTrack.Persistence.Employees;

public class EmployeeRepository(ApplicationDbContext context) : GenericRepository<Employee>(context), IEmployeeRepository;
