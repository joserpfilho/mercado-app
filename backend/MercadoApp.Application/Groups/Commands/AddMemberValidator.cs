using FluentValidation;
using MercadoApp.Application.Groups.DTOs;

namespace MercadoApp.Application.Groups.Commands;

public class AddMemberValidator : AbstractValidator<AddMemberRequest>
{
    public AddMemberValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-mail é obrigatório.")
            .EmailAddress().WithMessage("E-mail inválido.");
    }
}