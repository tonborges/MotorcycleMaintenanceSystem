using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Storage;
using Application.Common.Attributes;
using Domain.DeliveryPeople;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared.Results;

namespace Application.Features.DeliveryPeople.Create;

[RequireValidation]
public class CreateDeliveryPersonCommandHandler(
    IApplicationDbContext context,
    IValidator<CreateDeliveryPersonCommand> validator,
    ILogger<CreateDeliveryPersonCommandHandler> logger,
    IStorageService storageService)
    : ICommandHandler<CreateDeliveryPersonCommand>
{
    public async Task<ServiceResult> Handle(CreateDeliveryPersonCommand request, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("validating request");
            var resultValidation = await validator.ValidateAsync(request, cancellationToken);
            if (!resultValidation.IsValid)
                return ServiceResult.Fail(resultValidation.Errors.Select(e => e.ErrorMessage).ToList());

            logger.LogInformation("Validation passed");
            logger.LogInformation("Creating delivery person");
            var entity = (DeliveryPerson)request;
            logger.LogInformation("Delivery can ride motorcycle: {0}", entity.CannotRideMotorcycle());
            if (entity.CannotRideMotorcycle())
                return "Dados inválidos";

            logger.LogInformation("Setting drivers license image");
            entity.SetDriversLicenseImagem($"{Guid.NewGuid().ToString()}.{(request.DriversLicenseImage!.Contains("data:image/png;base64", StringComparison.InvariantCultureIgnoreCase) ? "png" : "bmp")}");

            logger.LogInformation("Uploading drivers license image");
            await storageService.UploadBase64FileAsync(request.DriversLicenseImage, entity.DriversLicenseImage)
                                .ConfigureAwait(false);

            logger.LogInformation("Adding delivery person to context");
            context.DeliveryPeople.Add(entity);

            logger.LogInformation("Saving changes");
            return await context.SaveChangesAsync(cancellationToken) > 0 ? ServiceResult.Ok() : "Dados inválidos";
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while creating a delivery person");
            return "Dados inválidos";
        }
    }
}