namespace LoanTrack.Application.Centers.Queries.Get;

public record GetCentersResponse(Guid Id,string Name, string? Description, string CenterLeader);
