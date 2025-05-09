namespace LoanTrack.Web.Shared.Groups;

public class GroupViewModel
{
    public Guid GroupId { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid CenterId { get; set; } = Guid.Empty;
    public string Center  { get; set; } = string.Empty;
}
