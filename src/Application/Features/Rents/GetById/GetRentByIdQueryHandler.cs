using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Results;

namespace Application.Features.Rents.GetById;

public sealed class GetRentByIdQueryHandler(
    IApplicationDbContext context,
    ILogger<GetRentByIdQueryHandler> logger)
    : IQueryHandler<GetRentByIdQuery, GetRentByIdResponse>
{
    public async Task<ServiceResult<GetRentByIdResponse>> Handle(GetRentByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("trying parse string to guid");
            if (Guid.TryParse(request.Id, out var id))
            {
                logger.LogInformation("Getting rent by id");
                var entity = await context.Rents.SingleOrDefaultAsync(s => s.Id == id, cancellationToken);
                logger.LogInformation("verifying if rent exists");
                if (entity is null)
                    return "Locação não encontrada";

                logger.LogInformation("Returning rent");
                return new ServiceResult<GetRentByIdResponse>(entity);
            }

            logger.LogInformation("Invalid id");
            return "Dados inválidos";
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting rent by id");
            return "Dados inválidos";
        }
    }
}