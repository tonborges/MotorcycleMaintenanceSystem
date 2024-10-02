using MediatR;
using Shared.Results;

namespace Application.Abstractions.Messaging;

public interface ICommandHandler<in TCommand>
    : IRequestHandler<TCommand, ServiceResult>
    where TCommand : ICommand;