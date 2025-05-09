using LoanTrack.Domain.Common;

namespace LoanTrack.Domain.Employees;

public static class EmployeeErrors
{
    public static Error NotFound(Guid userId) =>
        Error.NotFound("Employee.NotFound", $"The employee with the identifier {userId} not found");

    public static Error NotFound(string email) =>
        Error.NotFound("Employee.NotFound", $"The employee with the email {email} not found");
    
    public static Error NotFound() =>
        Error.NotFound("Employee.NotFound", $"Employees not found");
}
