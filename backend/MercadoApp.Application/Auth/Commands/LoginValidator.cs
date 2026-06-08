using FluentValidation;
using MercadoApp.Application.Auth.DTOs;

namespace MercadoApp.Application.Auth.Commands;

public class LoginValidator : AbstractValidator<LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-mail é obrigatório.")
            .EmailAddress().WithMessage("E-mail inválido.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Senha é obrigatória.");
    }
}