using FluentValidation;

namespace Application.Features.Motorcycles.UpdatePlate;

internal sealed class UpdateMotorcyclePlateCommandValidator : AbstractValidator<UpdateMotorcyclePlateCommand>
{
    public UpdateMotorcyclePlateCommandValidator()
    {
        RuleFor(x => x.Identifier)
            .NotEmpty().WithMessage("Dados inválidos");

        RuleFor(x => x.Plate)
            .NotEmpty().WithMessage("Dados inválidos");
    }
}