using FluentValidation;
using MercadoApp.Application.Items.DTOs;

namespace MercadoApp.Application.Items.Commands;

public class CreateItemValidator : AbstractValidator<CreateItemRequest>
{
    public CreateItemValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MaximumLength(100);

        RuleFor(x => x.Unit)
            .IsInEnum().WithMessage("Unidade inválida.");
    }
}