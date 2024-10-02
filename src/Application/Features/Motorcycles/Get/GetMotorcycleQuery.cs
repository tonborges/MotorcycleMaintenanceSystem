using Application.Abstractions.Messaging;

namespace Application.Features.Motorcycles.Get;

public record GetMotorcycleQuery(string? Plate) : IQuery<IEnumerable<GetMotorcycleResponse>>;