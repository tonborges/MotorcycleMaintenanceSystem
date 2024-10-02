using MediatR;
using Shared.Results;

namespace Application.Abstractions.Messaging;

public interface ICommand : IRequest<ServiceResult>, IBaseCommand;

public interface IBaseCommand;