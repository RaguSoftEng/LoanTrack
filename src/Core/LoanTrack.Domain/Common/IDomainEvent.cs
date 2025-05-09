using MediatR;

namespace LoanTrack.Domain.Common;

public interface IDomainEvent : INotification
{
    Guid Id { get; }

    DateTime OccurredOnUtc { get; }
}
