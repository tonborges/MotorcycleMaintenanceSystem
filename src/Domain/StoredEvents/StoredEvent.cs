namespace Domain.StoredEvents;

public class StoredEvent
{
    public Guid Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string? EventType { get; private set; }
    public string? Data { get; private set; }

    public StoredEvent()
    {
    }

    public StoredEvent(string? eventType, string? data)
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
        EventType = eventType;
        Data = data;
    }
}