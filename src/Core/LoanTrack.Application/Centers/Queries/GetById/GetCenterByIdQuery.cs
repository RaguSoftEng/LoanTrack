using LoanTrack.Application.Common.CQRS;

namespace LoanTrack.Application.Centers.Queries.GetById;

public record GetCenterByIdQuery(Guid CenterId) : IQuery<GetCenterByIdResponse>;
