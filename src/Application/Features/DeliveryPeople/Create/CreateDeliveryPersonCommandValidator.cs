using FluentValidation;

namespace Application.Features.DeliveryPeople.Create;

public class CreateDeliveryPersonCommandValidator : AbstractValidator<CreateDeliveryPersonCommand>
{
    public CreateDeliveryPersonCommandValidator()
    {
        RuleFor(x => x.Identifier)
            .NotEmpty().WithMessage("Dados inválidos");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Dados inválidos");

        RuleFor(x => x.Ein)
            .NotEmpty().WithMessage("Dados inválidos");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Dados inválidos");

        RuleFor(x => x.DriversLicenseNumber)
            .NotEmpty()
            .WithMessage("Dados inválidos");

        RuleFor(x => x.DriversLicenseType)
            .NotEmpty().WithMessage("Dados inválidos");

        RuleFor(x => x.DriversLicenseImage)
            .NotEmpty().WithMessage("Dados inválidos");
        When(x => x.DriversLicenseImage != null, () =>
        {
            RuleFor(x => x.DriversLicenseImage)
                .Must(x => x!.ToLowerInvariant().Contains("data:image/png;base64") || x!.ToLowerInvariant().Contains("data:image/bmp;base64")).WithMessage("Dados inválidos");
        });
    }
}