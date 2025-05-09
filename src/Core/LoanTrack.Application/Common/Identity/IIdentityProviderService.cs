using System.Security.Claims;
using LoanTrack.Application.Employees.Queries.Auth;
using LoanTrack.Domain.Common;

namespace LoanTrack.Application.Common.Identity;

public interface IIdentityProviderService
{
    Task<Result<string>> RegisterUserAsync(UserModel user, CancellationToken cancellationToken = default);
    Task UpdateUserRoleAsync(string userId, string role, CancellationToken cancellationToken = default);
    Task<Result<AuthResponse>> LoginAsync(AuthRequest auth, (Guid id, string name) employee, CancellationToken cancellationToken = default);
}
