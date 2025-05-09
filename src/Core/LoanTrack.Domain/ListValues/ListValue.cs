using LoanTrack.Domain.Common;

namespace LoanTrack.Domain.ListValues;

public sealed class ListValue : BaseEntity
{
    private string _listType = "";
    public string ListType
    {
        get => _listType;
       private set => _listType = ListTypes.Validate(value); 
        
    }
    
    public Guid ParentId { get; set; }
    public string Description { get; private set; }

    private ListValue(){}

    public static ListValue Create(string listType, string description, Guid parentId)
    => new() { ListType = listType, Description = description, ParentId = parentId };

    public void Update(string description)
    {
        Description = description;
    }
}
