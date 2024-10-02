using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Common.Attributes;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Results;

namespace Application.Features.Rents.UpdateReturn;

[RequireValidation]
public sealed class UpdateRentReturnCommandHandler(
    IApplicationDbContext context,
    IValidator<UpdateRentReturnCommand> validator,
    ILogger<UpdateRentReturnCommandHandler> logger)
    : ICommandHandler<UpdateRentReturnCommand>
{
    public async Task<ServiceResult> Handle(UpdateRentReturnCommand request, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("validating request");
            var resultValidation = await validator.ValidateAsync(request, cancellationToken);
            logger.LogInformation("Validation passed");
            if (!resultValidation.IsValid)
                return ServiceResult.Fail(resultValidation.Errors.Select(e => e.ErrorMessage).ToList());

            logger.LogInformation("trying parse string to guid");
            if (Guid.TryParse(request.Id, out var id))
            {
                logger.LogInformation("Finding rent");
                var entity = await context.Rents.SingleOrDefaultAsync(s => s.Id == id, cancellationToken);
                logger.LogInformation("Checking if rent exists");
                if (entity is null)
                    return "Dados inválidos";

                logger.LogInformation("Finalizing rental");
                entity.FinalizeRental(request.EndDate);

                logger.LogInformation("Saving changes");
                return await context.SaveChangesAsync(cancellationToken) > 0 ?
                    ServiceResult.Ok(["Data de devolução informada com sucesso"]) : 
                    "Dados inválidos";
            }

            logger.LogInformation("Invalid id");
            return "Dados inválidos";
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while updating a rent's return date");
            return "Dados inválidos";
        }
    }
}