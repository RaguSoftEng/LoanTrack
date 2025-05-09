using LoanTrack.Domain.Common;

namespace LoanTrack.Domain.Groups;

public class GroupErrors
{
    public static Error NotFound() =>
        Error.NotFound("Group.NotFound", $"Groups not found");
    public static Error NotFound(Guid groupId) =>
        Error.NotFound("Group.NotFound", $"The Group with the identifier {groupId} was not found");
    
    public static Error LeaderNotFound(Guid leaderId) =>
        Error.NotFound("Customers.NotFound", $"The customer with the identifier {leaderId} was not found"); 
}
