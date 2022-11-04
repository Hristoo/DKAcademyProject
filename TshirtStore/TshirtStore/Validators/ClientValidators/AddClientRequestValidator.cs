using FluentValidation;
using ThirtStore.Models.Models.Requests;

namespace TshirtStore.Validators.ClientValidators
{
    public class AddClientRequestValidator : AbstractValidator<ClientRequest>
    {
        public AddClientRequestValidator()
        {
            When(x => !string.IsNullOrEmpty(x.Name), () =>
            {
                RuleFor(x => x.Name).NotEmpty().WithMessage("The name is empty!");
                RuleFor(x => x.Name).MinimumLength(2).MaximumLength(30).WithMessage("The name must be between 2 and 30 symbols!");
            });
            RuleFor(x => x.Address).NotEmpty().WithMessage("The address is empty!");
        }
    }
}
