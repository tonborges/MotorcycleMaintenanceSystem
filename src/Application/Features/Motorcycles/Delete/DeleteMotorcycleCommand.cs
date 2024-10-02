using Application.Abstractions.Messaging;

namespace Application.Features.Motorcycles.Delete;

public record DeleteMotorcycleCommand(string? Id) : ICommand;