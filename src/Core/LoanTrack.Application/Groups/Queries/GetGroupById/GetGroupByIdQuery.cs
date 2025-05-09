using LoanTrack.Application.Common.CQRS;

namespace LoanTrack.Application.Groups.Queries.GetGroupById;

public record GetGroupByIdQuery(Guid GroupId) : IQuery<GetGroupByIdResponse>;
