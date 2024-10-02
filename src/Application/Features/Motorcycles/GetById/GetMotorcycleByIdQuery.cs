using Application.Abstractions.Messaging;

namespace Application.Features.Motorcycles.GetById;

public record GetMotorcycleByIdQuery(string? Id) : IQuery<GetMotorcycleByIdResponse>;