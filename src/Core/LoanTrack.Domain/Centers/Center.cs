using LoanTrack.Domain.Common;
using LoanTrack.Domain.Customers;

namespace LoanTrack.Domain.Centers;

public sealed class Center : AuditableEntity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Guid? CenterLeaderId { get; private set; }
    public Customer? CenterLeader { get; private set; }
    
    private Center() { }
    
    public static Center Create(string name, string description, Guid? centerLeaderId)
    {
        var group = new Center
        {
            Name = name,
            Description = description,
            CenterLeaderId = centerLeaderId == Guid.Empty ? null : centerLeaderId,
        };
        return group;
    }

    public void Update(string name, string description, Guid? centerLeaderId)
    {
        if (Name != name)
        {
            Name = name;
        }
        if (Description != description)
        {
            Description = description;
        }
        if (CenterLeaderId != centerLeaderId)
        {
            CenterLeaderId = centerLeaderId;
        }
    }
}
