using LoanTrack.Domain.Centers;
using LoanTrack.Domain.Common;

namespace LoanTrack.Domain.Groups;

public sealed class CustomerGroup : AuditableEntity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Guid CenterId { get; private set; }
    public Center Center { get; private set; }
    
    private CustomerGroup() { }
    
    public static CustomerGroup Create(string name, string description, Guid centerId)
    {
        var group = new CustomerGroup
        {
            Name = name,
            Description = description,
            CenterId = centerId
        };
        return group;
    }

    public void Update(string name, string description, Guid centerId)
    {
        if(Name != name) Name = name;
        if(Description != description) Description = description;
        if(CenterId != centerId) CenterId = centerId;
    }
}
