using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.DTOs;

namespace LoanTrack.Application.Employees.Queries.GetUsers;

public record GetUsersQuery() : QueryParameters, IQuery<PaginatedResult<GetUsersResponse>>;
