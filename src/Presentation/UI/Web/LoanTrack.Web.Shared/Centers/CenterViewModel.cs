namespace LoanTrack.Web.Shared.Centers;

public class CenterViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid LeaderId { get; set; } = Guid.Empty;
    public string Leader { get; set; }
}
