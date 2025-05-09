using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.DTOs;

namespace LoanTrack.Application.Centers.Queries.Get;

public record GetCentersQuery() :QueryParameters , IQuery<PaginatedResult<GetCentersResponse>>;
