using FluentValidation;
using ThirtStore.Models.Models.Requests;

namespace TshirtStore.Validators.TshirtValidators
{
    public class AddTshirtRequestValidator : AbstractValidator<TshirtRequest>
    {
        public AddTshirtRequestValidator()
        {
            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("The quantity must be greater '0'");
            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("The Price must be greater '0'");
            When(x => !string.IsNullOrEmpty(x.Size), () =>
            {
                RuleFor(x => x.Size).MinimumLength(1).MaximumLength(10);
            });
        }
    }
}
