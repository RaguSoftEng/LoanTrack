using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.DTOs;
using LoanTrack.Domain.Common;
using LoanTrack.Domain.Employees;

namespace LoanTrack.Application.Employees.Queries.GetUsers;

public class GetUsersQueryHandler(
    IEmployeeQueryRepository employeeQuery
)
    : IQueryHandler<GetUsersQuery, PaginatedResult<GetUsersResponse>>
{
    public async Task<Result<PaginatedResult<GetUsersResponse>>> Handle(GetUsersQuery request,
        CancellationToken cancellationToken)
    {
        var employees = await employeeQuery.GetListAsync(
            request.ToBaseQuery(),
            cancellationToken
        );

        return employees;
    }
}
