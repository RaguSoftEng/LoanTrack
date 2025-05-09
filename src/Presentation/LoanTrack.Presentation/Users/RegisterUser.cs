using LoanTrack.Application.Employees.Commands.RegisterUser;
using LoanTrack.Domain.Common;
using LoanTrack.Presentation.Common;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace LoanTrack.Presentation.Users;

internal sealed class RegisterUser : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
        => app.MapPost("users/register", async (Request request, ISender sender) =>
            {
                Result<Guid> result = await sender.Send(new RegisterUserCommand(
                    request.FirstName,
                    request.LastName,
                    request.Email,
                    request.Password,
                    request.Role
                ));

                return result.Match(Results.Ok, ApiResults.Problem);
            })
            .RequireAuthorization()
            .WithTags(ApiTags.Users);
    
    internal sealed class Request
    {
        public string Email { get; init; }

        public string Password { get; init; }

        public string FirstName { get; init; }

        public string LastName { get; init; }
        
        public string Role { get; init; }
    }
}
