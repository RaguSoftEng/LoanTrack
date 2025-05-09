using LoanTrack.Application.Customers.Queries.GetCustomer;
using LoanTrack.Application.Customers.Queries.GetCustomer.ById;
using LoanTrack.Domain.Common;
using LoanTrack.Presentation.Common;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace LoanTrack.Presentation.Customers;

public class GetCustomer:IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapGet("api/customer/{id}", async (Guid id, ISender sender) =>
            {
                Result<CustomerResponse> result = await sender.Send(new GetCustomerByIdQuery(id));
                return result.Match(Results.Ok, ApiResults.Problem);
            })
            .WithTags(ApiTags.Customers)
            .Produces<CustomerResponse>(StatusCodes.Status200OK);
}
