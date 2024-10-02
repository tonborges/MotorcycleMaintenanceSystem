using Shared.Domain;

namespace Domain.Motorcycles;

public sealed record MotorcycleCreatedDomainEvent(string? Identifier, int? Year, string? Model, string? Plate) : IDomainEvent;