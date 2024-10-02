using FluentValidation;

namespace Application.Features.Motorcycles.Delete;

internal sealed class DeleteMotorcycleCommandValidator : AbstractValidator<DeleteMotorcycleCommand>
{
    public DeleteMotorcycleCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("The Id field is mandatory.");
    }
}