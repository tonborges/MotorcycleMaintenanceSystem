using FluentValidation;

namespace Application.Features.Rents.UpdateReturn;

internal sealed class UpdateRentReturnCommandValidator : AbstractValidator<UpdateRentReturnCommand>
{
    public UpdateRentReturnCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Dados inválidos");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("Dados inválidos");
    }
}