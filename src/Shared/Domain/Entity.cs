namespace Shared.Domain;

public class Entity
{
    private readonly List<IDomainEvent> domainEvents = [];

    public List<IDomainEvent> DomainEvents => [.. domainEvents];
    
    public void ClearDomainEvents()
    {
        domainEvents.Clear();
    }

    public void Raise(IDomainEvent domainEvent)
    {
        domainEvents.Add(domainEvent);
    }
}