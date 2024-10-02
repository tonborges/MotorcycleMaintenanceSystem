using Application.Abstractions.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Motorcycles.Create;

internal sealed class CreateMotorcycleCommandValidator : AbstractValidator<CreateMotorcycleCommand>
{
    private readonly IApplicationDbContext applicationDbContext;

    public CreateMotorcycleCommandValidator(IApplicationDbContext applicationDbContext)
    {
        this.applicationDbContext = applicationDbContext;
        When(x => !string.IsNullOrWhiteSpace(x.Plate), () =>
        {
            RuleFor(x => x.Identifier)
                .NotEmpty().WithMessage("Dados inválidos");

            RuleFor(x => x.Identifier)
                .MustAsync(NotHaveSameIdentifier)
                .WithMessage("Dados inválidos");
        });

        RuleFor(x => x.Year)
            .NotEmpty().WithMessage("Dados inválidos");

        RuleFor(x => x.Model)
            .NotEmpty().WithMessage("Dados inválidos");

        When(x => !string.IsNullOrWhiteSpace(x.Plate), () =>
        {
            RuleFor(x => x.Plate)
                .NotEmpty().WithMessage("Dados inválidos");

            RuleFor(x => x.Plate)
                .MustAsync(NotHaveSamePlate)
                .WithMessage("Dados inválidos");
        });
    }

    private async Task<bool> NotHaveSamePlate(CreateMotorcycleCommand command, string? plate, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(plate))
            return true;

        var motorcycle = await applicationDbContext.Motorcycles.SingleOrDefaultAsync(x => x.Plate == plate, cancellationToken);
        if (motorcycle is null)
            return true;

        return false;
    }

    private async Task<bool> NotHaveSameIdentifier(CreateMotorcycleCommand command, string? identifier, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(identifier))
            return true;

        var motorcycle = await applicationDbContext.Motorcycles.SingleOrDefaultAsync(x => x.Identifier == identifier, cancellationToken);
        if (motorcycle is null)
            return true;

        return false;
    }
}