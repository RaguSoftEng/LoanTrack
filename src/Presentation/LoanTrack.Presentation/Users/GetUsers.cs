using LoanTrack.Application.Employees.Queries.GetUsers;
using LoanTrack.Domain.Common;
using LoanTrack.Presentation.Common;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace LoanTrack.Presentation.Users;

public class GetUsers : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapGet("api/users", async (ISender sender) =>
            {
                var result = await sender.Send(new GetUsersQuery());
                return result.Match(Results.Ok, ApiResults.Problem);
            })
          //  .RequireAuthorization()
            .WithTags(ApiTags.Users)
            .Produces<IReadOnlyCollection<GetUsersResponse>>(StatusCodes.Status200OK);
}
