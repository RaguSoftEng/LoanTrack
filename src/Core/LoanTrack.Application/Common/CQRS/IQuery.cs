using LoanTrack.Domain.Common;
using MediatR;

namespace LoanTrack.Application.Common.CQRS;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
