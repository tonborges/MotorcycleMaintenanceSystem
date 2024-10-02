using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Common.Attributes;
using Domain.Rents;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared.Results;

namespace Application.Features.Rents.Create;

[RequireValidation]
public sealed class CreateRentCommandHandler(
    IApplicationDbContext context,
    IValidator<CreateRentCommand> validator,
    ILogger<CreateRentCommandHandler> logger)
    : ICommandHandler<CreateRentCommand>
{
    public async Task<ServiceResult> Handle(CreateRentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("validating request");
            var resultValidation = await validator.ValidateAsync(request, cancellationToken);
            logger.LogInformation("Validation passed");
            if (!resultValidation.IsValid)
                return ServiceResult.Fail(resultValidation.Errors.Select(e => e.ErrorMessage).ToList());

            logger.LogInformation("Creating rent");
            var entity = (Rent)request;
            context.Rents.Add(entity);

            logger.LogInformation("Saving changes");
            return await context.SaveChangesAsync(cancellationToken) > 0 ? ServiceResult.Ok().SetId(entity.Id) : "Dados inválidos";
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while creating a rent");
            return "Dados inválidos";
        }
    }
}