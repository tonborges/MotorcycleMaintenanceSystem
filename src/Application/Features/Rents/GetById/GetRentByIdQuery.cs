using Application.Abstractions.Messaging;

namespace Application.Features.Rents.GetById;

public record GetRentByIdQuery(string? Id) : IQuery<GetRentByIdResponse>;