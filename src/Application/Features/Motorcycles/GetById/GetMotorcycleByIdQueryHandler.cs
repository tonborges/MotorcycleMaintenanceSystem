using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Results;

namespace Application.Features.Motorcycles.GetById;

public sealed class GetMotorcycleByIdQueryHandler(
    IApplicationDbContext context,
    ILogger<GetMotorcycleByIdQueryHandler> logger)
    : IQueryHandler<GetMotorcycleByIdQuery, GetMotorcycleByIdResponse>
{
    public async Task<ServiceResult<GetMotorcycleByIdResponse>> Handle(GetMotorcycleByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Getting motorcycle by id");
            var entity = await context.Motorcycles.SingleOrDefaultAsync(s => s.Identifier! == request.Id, cancellationToken);

            logger.LogInformation("verifying if motorcycle exists");
            if (entity is null)
                return new ServiceResult<GetMotorcycleByIdResponse>(true, data: null, ["Moto não encontrada"]);

            logger.LogInformation("Returning motorcycle");
            return new ServiceResult<GetMotorcycleByIdResponse>(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting motorcycle by id");
            return "Request mal formada";
        }
    }
}