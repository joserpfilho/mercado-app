using FluentValidation;
using MercadoApp.Application.Groups.DTOs;

namespace MercadoApp.Application.Groups.Commands;

public class CreateGroupValidator : AbstractValidator<CreateGroupRequest>
{
    public CreateGroupValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MaximumLength(100);
    }
}