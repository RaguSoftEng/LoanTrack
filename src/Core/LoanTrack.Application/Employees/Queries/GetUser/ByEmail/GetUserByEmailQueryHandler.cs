using LoanTrack.Application.Common.CQRS;
using LoanTrack.Domain.Common;
using LoanTrack.Domain.Employees;

namespace LoanTrack.Application.Employees.Queries.GetUser.ByEmail;

public class GetUserByEmailQueryHandler(IEmployeeQueryRepository repository)
: IQueryHandler<GetUserByEmailQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var res = await repository.GetSignInEmployeeAsync(request.Email, cancellationToken);
        if (res is null)
        {
            return Result.Failure<UserResponse>(EmployeeErrors.NotFound(request.Email));
        }

        UserResponse? user = new UserResponse(
            res.UserId,
            res.FirstName,
            res.LastName,
            res.Email,
            res.UserRole,
            true
        );

        return user;
    }
}
