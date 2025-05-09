namespace LoanTrack.Application.Centers.Queries.GetById;

public record GetCenterByIdResponse(Guid CenterId, string Name, string? Description, Guid CenterLeaderId, string CenterLeader);
