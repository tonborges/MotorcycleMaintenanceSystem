using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Motorcycles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Results;

namespace Application.Features.Motorcycles.Get;

public sealed class GetMotorcycleQueryHandler(
    IApplicationDbContext context,
    ILogger<GetMotorcycleQueryHandler> logger)
    : IQueryHandler<GetMotorcycleQuery, IEnumerable<GetMotorcycleResponse>>
{
    public async Task<ServiceResult<IEnumerable<GetMotorcycleResponse>>> Handle(GetMotorcycleQuery request, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Getting motorcycles");
            List<Motorcycle> motorcycles;
            if (string.IsNullOrWhiteSpace(request.Plate))
                motorcycles = await context
                                    .Motorcycles
                                    .ToListAsync(cancellationToken);
            else
                motorcycles = await context
                                    .Motorcycles
                                    .Where(s => s.Plate == request.Plate)
                                    .ToListAsync(cancellationToken);

            logger.LogInformation("Returning motorcycles");
            return new ServiceResult<IEnumerable<GetMotorcycleResponse>>(motorcycles.Select(s => (GetMotorcycleResponse)s!));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting motorcycles");
            return new ServiceResult<IEnumerable<GetMotorcycleResponse>>(Enumerable.Empty<GetMotorcycleResponse>());
        }
    }
}