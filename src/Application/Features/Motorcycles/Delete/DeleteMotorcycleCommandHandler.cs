using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Common.Attributes;
using Domain.Motorcycles;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Results;

namespace Application.Features.Motorcycles.Delete;

[RequireValidation]
public sealed class DeleteMotorcycleCommandHandler(
    IApplicationDbContext context,
    IValidator<DeleteMotorcycleCommand> validator,
    ILogger<DeleteMotorcycleCommandHandler> logger)
    : ICommandHandler<DeleteMotorcycleCommand>
{
    public async Task<ServiceResult> Handle(DeleteMotorcycleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("validating request");
            var resultValidation = await validator.ValidateAsync(request, cancellationToken);
            if (!resultValidation.IsValid)
                return ServiceResult.Fail(resultValidation.Errors.Select(e => e.ErrorMessage).ToList());

            logger.LogInformation("Validation passed");
            logger.LogInformation("Finding motorcycle");
            var entity = await context
                               .Motorcycles
                               .Include(x => x.Rents)
                               .SingleOrDefaultAsync(s => s.Identifier == request.Id, cancellationToken);

            logger.LogInformation("Checking if motorcycle exists");
            if (entity is null)
                return "Dados inválidos";

            logger.LogInformation("Checking if motorcycle has rents");
            if (entity.Rents.Any())
                return "Dados inválidos";

            logger.LogInformation("Raised MotorcycleDeletedDomainEvent");
            entity.Raise(new MotorcycleDeletedDomainEvent(entity.Identifier, entity.Year, entity.Model, entity.Plate));

            logger.LogInformation("Removing motorcycle from context");
            context.Motorcycles.Remove(entity);

            logger.LogInformation("Saving changes");
            return await context.SaveChangesAsync(cancellationToken) > 0 ? 
                ServiceResult.Ok() : 
                "Dados inválidos";
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while deleting a motorcycle");
            return "Dados inválidos";
        }
    }
}