using FluentValidation;
using ThirtStore.Models.Models;

namespace TshirtStore.Validators.TshirtValidators
{
    public class UpdateTshirtRequestValidator : AbstractValidator<Tshirt>
    {
        public UpdateTshirtRequestValidator()
        {
            RuleFor(x => x.Quantity)
               .GreaterThan(0)
               .WithMessage("The quantity must be greater '0'");
            RuleFor(x => x.Id)
                .NotEmpty();
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
