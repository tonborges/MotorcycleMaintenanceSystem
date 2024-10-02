using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Common.Attributes;
using Domain.Motorcycles;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Results;

namespace Application.Features.Motorcycles.UpdatePlate;

[RequireValidation]
public sealed class UpdateMotorcyclePlateCommandHandler(
    IApplicationDbContext context,
    IValidator<UpdateMotorcyclePlateCommand> validator,
    ILogger<UpdateMotorcyclePlateCommandHandler> logger)
    : ICommandHandler<UpdateMotorcyclePlateCommand>
{
    public async Task<ServiceResult> Handle(UpdateMotorcyclePlateCommand request, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("validating request");
            var resultValidation = await validator.ValidateAsync(request, cancellationToken);
            if (!resultValidation.IsValid)
                return ServiceResult.Fail(resultValidation.Errors.Select(e => e.ErrorMessage).ToList());

            logger.LogInformation("Validation passed");
            logger.LogInformation("Finding motorcycle");
            var entity = await context.Motorcycles.SingleOrDefaultAsync(s => s.Identifier! == request.Identifier, cancellationToken);
            logger.LogInformation("Checking if motorcycle exists");
            if (entity is null)
                return "Dados inválidos";

            logger.LogInformation("Setting motorcycle plate");
            entity.SetPlate(request.Plate);

            logger.LogInformation("Raised MotorcycleUpdatedDomainEvent");
            entity.Raise(new MotorcycleUpdatedDomainEvent(entity.Identifier, entity.Plate));

            logger.LogInformation("Updating motorcycle's plate");
            logger.LogInformation("Saving changes");
            return await context.SaveChangesAsync(cancellationToken) > 0 ? ServiceResult.Ok(["Placa modificada com sucesso"]) : "Dados inválidos";
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while updating a motorcycle's plate");
            return "Dados inválidos";
        }
    }
}