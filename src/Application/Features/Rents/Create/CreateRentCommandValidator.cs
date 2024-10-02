using FluentValidation;

namespace Application.Features.Rents.Create;

internal sealed class CreateRentCommandValidator : AbstractValidator<CreateRentCommand>
{
    public CreateRentCommandValidator()
    {
        RuleFor(x => x.DeliveryPersonId)
            .NotEmpty().WithMessage("Dados inválidos");

        RuleFor(x => x.MotorcycleId)
            .NotEmpty().WithMessage("Dados inválidos");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Dados inválidos");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("Dados inválidos");

        RuleFor(x => x.ForecastDateEnd)
            .NotEmpty().WithMessage("Dados inválidos");

        RuleFor(x => x.Plan)
            .NotEmpty().WithMessage("Dados inválidos");
    }
}