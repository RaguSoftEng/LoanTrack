using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.DTOs;
using LoanTrack.Application.Customers.Queries.GetCustomer;
using LoanTrack.Application.Dashboard;
using LoanTrack.Domain.Common;

namespace LoanTrack.Application.Customers.Queries;

public interface ICustomerQueryRepository
{
    Task<CustomerResponse?> GetByNicAsync(string nic, CancellationToken cancellationToken = default);
    Task<CustomerResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PaginatedResult<CustomerResponse>> GetCustomersByFilter(
        QueryParameters queryParams,
        Guid centerId,
        Guid groupId,
        CancellationToken cancellationToken = default
    );
    Task<Guid?> GetCustomerIdByNicAsync(string nic, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<(string Center, string Group, int Count)>> GetCustomersCountsByCenter(
        CancellationToken cancellationToken = default   
    );
}
