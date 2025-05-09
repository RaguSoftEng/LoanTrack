using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.DTOs;
using LoanTrack.Application.Employees.Queries;
using LoanTrack.Application.Employees.Queries.Auth;
using LoanTrack.Application.Employees.Queries.GetUsers;
using LoanTrack.Persistence.Common.Database;
using LoanTrack.Persistence.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace LoanTrack.Persistence.Employees;

public class EmployeeQueryRepository(ApplicationDbContext context) : IEmployeeQueryRepository
{
    public async Task<bool> IsEmailExistAsync(string email, CancellationToken token = default)
        => await context.Users.AnyAsync(x=>x.Email == email, token);

    public async Task<bool> IsActiveAsync(Guid id, CancellationToken token = default)
        => await context.Users.AnyAsync(x => x.Id == id && x.IsActive, token);

    public async Task<AuthEmployeeResponse?> GetSignInEmployeeAsync(string email, CancellationToken token = default)
        => await context.Users
            .Where(x => x.Email == email && x.IsActive)
            .Select(x => new AuthEmployeeResponse(x.Id, x.FirstName, x.LastName, x.Email, x.Role, x.IdentityId))
            .FirstOrDefaultAsync(token);

    public async Task<PaginatedResult<GetUsersResponse>> GetListAsync(
        QueryParameters queryParams,
        CancellationToken token = default
    )
    {
        var query = context.Users
            .SmartSearch(queryParams.SearchBy, queryParams.Search);

        var count = await query.CountAsync(token);

        var results = await query
            .SmartSort(queryParams.SortBy, queryParams.SortDescending)
            .SmartPaging(queryParams.Page, queryParams.PageSize)
            .Select(x => new GetUsersResponse(x.Id, x.FirstName, x.LastName, x.Email, x.Role, x.IsActive))
            .ToListAsync(token);

        return new PaginatedResult<GetUsersResponse>
        {
            Items = results,
            TotalCount = count,
            Page = queryParams.Page,
            PageSize = queryParams.PageSize
        };
    }
}
