using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.Identity;
using LoanTrack.Domain.Common;
using LoanTrack.Domain.Employees;

namespace LoanTrack.Application.Employees.Queries.Auth;

public class AuthHandler(
    IIdentityProviderService identityProvider,
    IEmployeeQueryRepository employeeQuery
) : IQueryHandler<AuthRequest, AuthResponse>
{
    public async Task<Result<AuthResponse>> Handle(AuthRequest request, CancellationToken cancellationToken)
    {
        var employee = await employeeQuery.GetSignInEmployeeAsync(request.Email, cancellationToken);
        if (employee == null)
        {
            return Result.Failure<AuthResponse>(EmployeeErrors.NotFound(request.Email));
        }
        
        var response = await identityProvider.LoginAsync(
            request,
            (employee.UserId,$"{employee.FirstName} {employee.LastName}"),
            cancellationToken
        );
        return response;
    }
}
