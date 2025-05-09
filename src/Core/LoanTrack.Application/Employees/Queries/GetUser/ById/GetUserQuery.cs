using LoanTrack.Application.Common.CQRS;

namespace LoanTrack.Application.Employees.Queries.GetUser.ById;

public sealed record GetUserQuery(Guid UserId) : IQuery<UserResponse>;
