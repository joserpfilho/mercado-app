using FluentValidation;
using MercadoApp.Application.ShoppingLists.DTOs;

namespace MercadoApp.Application.ShoppingLists.Commands;

public class CreateShoppingListValidator : AbstractValidator<CreateShoppingListRequest>
{
    public CreateShoppingListValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MaximumLength(100);
    }
}