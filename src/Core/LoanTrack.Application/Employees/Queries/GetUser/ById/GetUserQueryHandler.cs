using LoanTrack.Application.Common.CQRS;
using LoanTrack.Domain.Common;
using LoanTrack.Domain.Employees;

namespace LoanTrack.Application.Employees.Queries.GetUser.ById;

internal sealed class GetUserQueryHandler(IEmployeeRepository employeeRepository)
    : IQueryHandler<GetUserQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var res = await employeeRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (res is null)
        {
            return Result.Failure<UserResponse>(EmployeeErrors.NotFound(request.UserId));
        }

        UserResponse? user = new UserResponse(
            res.Id,
            res.FirstName,
            res.LastName,
            res.Email,
            res.Role,
            res.IsActive
        );

        return user;
    }
}
