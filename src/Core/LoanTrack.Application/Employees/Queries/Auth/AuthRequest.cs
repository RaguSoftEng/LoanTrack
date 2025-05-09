using LoanTrack.Application.Common.CQRS;

namespace LoanTrack.Application.Employees.Queries.Auth;

public record AuthRequest(string Email, string Password) : IQuery<AuthResponse>;
