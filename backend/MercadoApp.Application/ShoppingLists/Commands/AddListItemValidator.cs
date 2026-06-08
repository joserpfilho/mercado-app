using FluentValidation;
using MercadoApp.Application.ShoppingLists.DTOs;

namespace MercadoApp.Application.ShoppingLists.Commands;

public class AddListItemValidator : AbstractValidator<AddListItemRequest>
{
    public AddListItemValidator()
    {
        RuleFor(x => x.ItemId)
            .NotEmpty().WithMessage("Item é obrigatório.");

        RuleFor(x => x.DepartmentId)
            .NotEmpty().WithMessage("Departamento é obrigatório.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantidade deve ser maior que zero.");
    }
}