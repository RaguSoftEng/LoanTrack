using Microsoft.AspNetCore.Routing;

namespace LoanTrack.Presentation.Common;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);  
}
