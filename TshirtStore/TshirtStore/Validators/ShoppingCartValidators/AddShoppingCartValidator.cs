using FluentValidation;
using ThirtStore.Models.Models.Requests;

namespace TshirtStore.Validators.ShoppingCartValidators
{
    public class AddShoppingCartValidator : AbstractValidator<ShoppingCartRequest>
    {
        public AddShoppingCartValidator()
        {
            RuleFor(x => x.ClientId).NotEmpty();

            RuleForEach(x => x.Tshirts).ChildRules(tshirts =>
            {
                tshirts.RuleFor(t => t.Quantity).GreaterThan(0).WithMessage("Tshirts quantity must be greater than 0!");
                tshirts.RuleFor(t => t.Color).NotEmpty().WithMessage("Tshirts color is required");
                tshirts.RuleFor(t => t.Size).NotEmpty().WithMessage("Tshirts size is required");
            });
        }
    }
}
