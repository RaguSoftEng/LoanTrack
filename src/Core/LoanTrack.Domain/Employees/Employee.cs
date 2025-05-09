using LoanTrack.Domain.Common;

namespace LoanTrack.Domain.Employees;

public class Employee: AuditableEntity
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string IdentityId { get; private set; }
    public bool IsActive { get; private set; }
    public string Role { get; private set; }
    
    private Employee(){}
    
    public static Employee Create(
        string firstName,
        string lastName,
        string email,
        string identityId,
        string role
    )
    {
        var user = new Employee
        {
            Id = Guid.NewGuid(),
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            IdentityId = identityId,
            IsActive = true,
            Role = role
        };
        return user;
    }

    public void UpdateProfile(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
}
