using LoanTrack.Domain.Common;
using MediatR;

namespace LoanTrack.Application.Common.CQRS;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>;
