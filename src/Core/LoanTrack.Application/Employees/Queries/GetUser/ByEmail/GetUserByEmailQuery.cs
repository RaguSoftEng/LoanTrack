using LoanTrack.Application.Common.CQRS;

namespace LoanTrack.Application.Employees.Queries.GetUser.ByEmail;

public record GetUserByEmailQuery(string Email) : IQuery<UserResponse>;
