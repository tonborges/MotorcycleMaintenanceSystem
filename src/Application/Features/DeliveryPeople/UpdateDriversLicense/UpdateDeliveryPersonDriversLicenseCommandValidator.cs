using FluentValidation;

namespace Application.Features.DeliveryPeople.UpdateDriversLicense;

public class UpdateDeliveryPersonDriversLicenseCommandValidator : AbstractValidator<UpdateDeliveryPersonDriversLicenseCommand>
{
    public UpdateDeliveryPersonDriversLicenseCommandValidator()
    {
        RuleFor(x => x.Identifier)
            .NotEmpty().WithMessage("Dados inválidos");

        RuleFor(x => x.DriversLicenseImage)
            .NotEmpty().WithMessage("Dados inválidos")
            .Must(x => x!.ToLowerInvariant().Contains("data:image/png;base64") || x!.ToLowerInvariant().Contains("data:image/bmp;base64")).WithMessage("Dados inválidos");
    }
}
