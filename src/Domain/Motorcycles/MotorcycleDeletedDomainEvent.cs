using Shared.Domain;

namespace Domain.Motorcycles;

public sealed record MotorcycleDeletedDomainEvent(string? Identifier, int? Year, string? Model, string? Plate) : IDomainEvent;