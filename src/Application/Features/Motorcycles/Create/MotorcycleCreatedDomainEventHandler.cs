using Domain.Motorcycles;
using MassTransit;
using MediatR;

namespace Application.Features.Motorcycles.Create;

internal sealed class MotorcycleCreatedDomainEventHandler(IPublishEndpoint publishEndpoint) 
    : INotificationHandler<MotorcycleCreatedDomainEvent>
{
    public async Task Handle(MotorcycleCreatedDomainEvent notification, CancellationToken cancellationToken)
        => await publishEndpoint.Publish(notification, cancellationToken);
}