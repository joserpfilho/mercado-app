using FluentValidation;
using MercadoApp.Application.Departments.DTOs;

namespace MercadoApp.Application.Departments.Commands;

public class CreateDepartmentValidator : AbstractValidator<CreateDepartmentRequest>
{
    public CreateDepartmentValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MaximumLength(100);

        RuleFor(x => x.Icon)
            .NotEmpty().WithMessage("Ícone é obrigatório.")
            .MaximumLength(10);
    }
}