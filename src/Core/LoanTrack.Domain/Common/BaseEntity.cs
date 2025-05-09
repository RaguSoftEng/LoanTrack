namespace LoanTrack.Domain.Common;

public abstract class BaseEntity
{
    public Guid Id { get; protected init; }
    
    protected BaseEntity(){}
    
    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents;

    public void ClearDomainEvents() => _domainEvents.Clear();

    protected void Raise(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}
