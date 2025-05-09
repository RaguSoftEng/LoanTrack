using LoanTrack.Domain.Common;
using MediatR;

namespace LoanTrack.Application.Common.CQRS;

public interface ICommand : IRequest<Result>, IBaseCommand;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand;

public interface IBaseCommand;
