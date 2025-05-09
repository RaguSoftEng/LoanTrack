using LoanTrack.Domain.Common;
using MediatR;

namespace LoanTrack.Application.Common.CQRS;

public interface IDomainEventHandler<in TDomainEvent> : INotificationHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent;
