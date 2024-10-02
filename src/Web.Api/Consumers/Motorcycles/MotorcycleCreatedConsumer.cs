using System.Text.Encodings.Web;
using System.Text.Json;
using Application.Abstractions.Data;
using Domain.Motorcycles;
using Domain.StoredEvents;
using MassTransit;

namespace Web.Api.Consumers.Motorcycles;

public class MotorcycleCreatedConsumer(IApplicationDbContext applicationDbContext) : IConsumer<MotorcycleCreatedDomainEvent>
{
    public async Task Consume(ConsumeContext<MotorcycleCreatedDomainEvent> context)
    {
        if (context.Message.Year == 2024)
        {
            var storedEvent = new StoredEvent("MotorcycleCreated", JsonSerializer.Serialize(context.Message, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping }));
            applicationDbContext.StoredEvents.Add(storedEvent);
            await applicationDbContext.SaveChangesAsync();
        }
    }
}