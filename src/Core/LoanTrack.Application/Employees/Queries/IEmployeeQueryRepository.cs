using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.DTOs;
using LoanTrack.Application.Employees.Queries.Auth;
using LoanTrack.Application.Employees.Queries.GetUsers;

namespace LoanTrack.Application.Employees.Queries;

public interface IEmployeeQueryRepository
{
    Task<bool> IsEmailExistAsync(string email, CancellationToken token = default);
    Task<bool> IsActiveAsync(Guid id, CancellationToken token = default);
    Task<AuthEmployeeResponse?> GetSignInEmployeeAsync(string email, CancellationToken token = default);
    Task<PaginatedResult<GetUsersResponse>> GetListAsync(QueryParameters queryParams, CancellationToken token = default);
}
