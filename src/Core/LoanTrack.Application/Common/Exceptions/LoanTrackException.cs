using LoanTrack.Domain.Common;

namespace LoanTrack.Application.Common.Exceptions;

public class LoanTrackException(
    string requestName,
    Error? error = default,
    Exception? innerException = default
) : Exception("Application exception", innerException)
{
    public string RequestName { get; } = requestName;

    public Error? Error { get; } = error;
}
