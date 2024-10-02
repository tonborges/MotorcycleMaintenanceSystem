using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Common.Attributes;
using Domain.Motorcycles;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared.Results;

namespace Application.Features.Motorcycles.Create;

[RequireValidation]
public sealed class CreateMotorcycleCommandHandler(
    IApplicationDbContext context,
    IValidator<CreateMotorcycleCommand> validator,
    ILogger<CreateMotorcycleCommandHandler> logger)
    : ICommandHandler<CreateMotorcycleCommand>
{
    public async Task<ServiceResult> Handle(CreateMotorcycleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("validating request");
            var resultValidation = await validator.ValidateAsync(request, cancellationToken);
            if (!resultValidation.IsValid)
                return ServiceResult.Fail(resultValidation.Errors.Select(e => e.ErrorMessage).ToList());

            logger.LogInformation("Validation passed");
            logger.LogInformation("Creating motorcycle");
            var entity = (Motorcycle)request;

            logger.LogInformation("Raised MotorcycleCreatedDomainEvent");
            entity.Raise(new MotorcycleCreatedDomainEvent(entity.Identifier, entity.Year, entity.Model, entity.Plate));

            logger.LogInformation("Adding motorcycle to context");
            context.Motorcycles.Add(entity);

            logger.LogInformation("Saving changes");
            return await context.SaveChangesAsync(cancellationToken) > 0 ? ServiceResult.Ok() : "Dados inválidos";
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while creating a motorcycle");
            return "Dados inválidos";
        }
    }
}