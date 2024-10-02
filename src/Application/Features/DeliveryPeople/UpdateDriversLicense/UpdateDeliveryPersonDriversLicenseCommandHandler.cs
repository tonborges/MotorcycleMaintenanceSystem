using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Storage;
using Application.Common.Attributes;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Results;

namespace Application.Features.DeliveryPeople.UpdateDriversLicense;

[RequireValidation]
public class UpdateDeliveryPersonDriversLicenseCommandHandler(
    IApplicationDbContext context,
    IValidator<UpdateDeliveryPersonDriversLicenseCommand> validator,
    ILogger<UpdateDeliveryPersonDriversLicenseCommandHandler> logger,
    IStorageService storageService)
    : ICommandHandler<UpdateDeliveryPersonDriversLicenseCommand>
{
    public async Task<ServiceResult> Handle(UpdateDeliveryPersonDriversLicenseCommand request, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("validating request");
            var resultValidation = await validator.ValidateAsync(request, cancellationToken);
            if (!resultValidation.IsValid)
                return ServiceResult.Fail(resultValidation.Errors.Select(e => e.ErrorMessage).ToList());

            logger.LogInformation("Validation passed");
            logger.LogInformation("finding delivery person");
            var entity = await context.DeliveryPeople.SingleOrDefaultAsync(s => s.Identifier! == request.Identifier, cancellationToken);
            if (entity is null)
                return "Dados inválidos";

            logger.LogInformation("Removing old drivers license image");
            await storageService.RemoveFileAsync(entity.DriversLicenseImage)
                                .ConfigureAwait(false);
            
            logger.LogInformation("Setting drivers license image");
            entity.SetDriversLicenseImagem($"{Guid.NewGuid().ToString()}.{(request.DriversLicenseImage!.Contains("data:image/png;base64", StringComparison.InvariantCultureIgnoreCase) ? "png" : "bmp")}");

            logger.LogInformation("Uploading drivers license image");
            await storageService.UploadBase64FileAsync(request.DriversLicenseImage, entity.DriversLicenseImage)
                                .ConfigureAwait(false);

            logger.LogInformation("Updating delivery person's driver's license");
            logger.LogInformation("Saving changes");
            return await context.SaveChangesAsync(cancellationToken) > 0 ? ServiceResult.Ok() : "Dados inválidos";
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while updating a delivery person's driver's license");
            return "Dados inválidos";
        }
    }
}