namespace LoanTrack.Domain.Common;

public class DomainEvent : IDomainEvent
{
    public Guid Id { get; }
    public DateTime OccurredOnUtc { get; }

    protected DomainEvent(Guid id, DateTime occurredOnUtc)
    {
        Id = id;
        OccurredOnUtc = occurredOnUtc;
    }

    protected DomainEvent()
    {
        Id = Guid.NewGuid();
        OccurredOnUtc = DateTime.UtcNow;
    }
}
