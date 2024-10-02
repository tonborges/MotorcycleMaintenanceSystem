namespace Shared.Results;

public class Notification
{
    public string? Message { get; set; }
    public string? Property { get; set; }

    public Notification(string? message, string? property)
    {
        Message = message;
        Property = property;
    }
}
