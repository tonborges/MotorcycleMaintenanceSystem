using MediatR;
using Shared.Results;

namespace Application.Abstractions.Messaging;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, ServiceResult<TResponse>>
    where TQuery : IQuery<TResponse>;
