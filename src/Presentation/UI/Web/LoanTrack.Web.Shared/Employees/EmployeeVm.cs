using System.ComponentModel.DataAnnotations;
using LoanTrack.Application.Employees.Queries.GetUsers;
using LoanTrack.Domain.Common.Constants;

namespace LoanTrack.Web.Shared.Employees;

public class EmployeeVm
{
    public Guid UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string UserRole { get; set; } = EmployeeRoles.Employee;
    
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters")]
    public string Password { get; set; }


    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; }
    public bool IsActive { get; set; }

    public static IReadOnlyList<EmployeeVm> LoadEmployees(IReadOnlyList<GetUsersResponse> users)
    {
        return
        [
            .. users.Select(x => new EmployeeVm()
            {
                UserId = x.UserId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                UserRole = x.UserRole,
                IsActive = x.IsActive
            })
        ];
    }
}
