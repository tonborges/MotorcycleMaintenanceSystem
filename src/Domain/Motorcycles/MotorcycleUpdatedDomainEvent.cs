using Shared.Domain;

namespace Domain.Motorcycles;

public sealed record MotorcycleUpdatedDomainEvent(string? Identifier, string? Plate) : IDomainEvent;