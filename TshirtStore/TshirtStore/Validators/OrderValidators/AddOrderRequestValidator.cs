using FluentValidation;
using ThirtStore.Models.Models.Requests;

namespace TshirtStore.Validators.OrderValidators
{
    public class AddOrderRequestValidator : AbstractValidator<OrderRequest>
    {
        public AddOrderRequestValidator()
        {
            RuleFor(x => x.ClientId)
                .NotNull()
                .WithMessage("Client id is requred!");
            RuleFor(x => x.Sum).GreaterThan(0);
            RuleFor(x => x.Tshirts)
                .NotNull();
            RuleFor(x => x.ClientId).NotEmpty();
        }
    }
}
