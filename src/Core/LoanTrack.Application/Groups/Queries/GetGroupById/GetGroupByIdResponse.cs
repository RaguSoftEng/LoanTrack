namespace LoanTrack.Application.Groups.Queries.GetGroupById;

public record GetGroupByIdResponse(Guid Id, string Name, string Description, string Center, Guid CenterId);
