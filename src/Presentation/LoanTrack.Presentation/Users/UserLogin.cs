using LoanTrack.Application.Employees.Queries.Auth;
using LoanTrack.Domain.Common;
using LoanTrack.Presentation.Common;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace LoanTrack.Presentation.Users;

public class UserLogin:IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
        => app.MapPost("/api/users/login", async (Request request, ISender sender) =>
            {
                Result<AuthResponse> result = await sender.Send(new AuthRequest(request.Email, request.Password));
                return result.Match(Results.Ok, ApiResults.Problem);
            })
            .AllowAnonymous()
            .WithTags(ApiTags.Users);
    
    internal sealed class Request
    {
        public string Email { get; init; }

        public string Password { get; init; }
    }
}
