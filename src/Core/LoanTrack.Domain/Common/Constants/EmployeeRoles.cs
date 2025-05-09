namespace LoanTrack.Domain.Common.Constants;

public static class EmployeeRoles
{
    public const string Admin  = "Admin";
    public const string Employee = "Employee";
    public const string Manager = "Manager";
    
    private static readonly HashSet<string> ValidRoles = [Admin, Employee, Manager];
    
    public static string Validate(string value)
        => ValidRoles.Contains(value)
            ? value : throw new ArgumentException("Invalid Roles");
    
    public static IReadOnlyCollection<string> GetRoles => [.. ValidRoles];
}
