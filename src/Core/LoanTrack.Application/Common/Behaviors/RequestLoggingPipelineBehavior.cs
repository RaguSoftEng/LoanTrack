using LoanTrack.Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LoanTrack.Application.Common.Behaviors;

internal sealed class RequestLoggingPipelineBehavior<TRequest, TResponse>(
    ILogger<RequestLoggingPipelineBehavior<TRequest, TResponse>> logger
) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
    where TResponse : Result
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        string moduleName = GetModuleName(typeof(TRequest).FullName!);
        string requestName = typeof(TRequest).Name;

        using (logger.BeginScope(new Dictionary<string, object>
           {
               ["Module"] = moduleName
           })
        )
        {
            logger.LogInformation("Processing request {RequestName}", requestName);

            TResponse result = await next();

            if (result.IsSuccess)
            {
                logger.LogInformation("Completed request {RequestName}", requestName);
            }
            else
            {
                logger.LogError("Completed request {RequestName} with error: {Error}", requestName, result.Error);
            }

            return result;
        }
    }

    private static string GetModuleName(string requestName) => requestName.Split('.')[2];
}
