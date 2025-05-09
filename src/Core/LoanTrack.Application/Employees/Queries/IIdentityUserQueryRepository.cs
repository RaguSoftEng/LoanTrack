namespace LoanTrack.Application.Employees.Queries;

public interface IIdentityUserQueryRepository
{
    Task<string> GetUserRoleByUserIdAsync(Guid userId);
}
