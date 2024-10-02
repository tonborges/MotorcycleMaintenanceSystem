using MediatR;
using Shared.Results;

namespace Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<ServiceResult<TResponse>>;
