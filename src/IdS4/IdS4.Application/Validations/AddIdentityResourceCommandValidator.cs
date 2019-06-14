using FluentValidation;
using IdS4.Application.Commands;

namespace IdS4.Application.Validations
{
    public class AddIdentityResourceCommandValidator: AbstractValidator<AddIdentityResourceCommand>
    {
        public AddIdentityResourceCommandValidator()
        {
            RuleFor(s => s.Resource.Name)
                .NotEmpty()
                .WithMessage("resource name should not be null")
                .MaximumLength(32)
                .WithMessage("length of resource name should less than or equal 32");

            RuleFor(s=>s.Resource.DisplayName)
                .NotEmpty()
                .WithMessage("resource name should not be null")
                .MaximumLength(32)
                .WithMessage("length of resource name should less than or equal 32");

            RuleFor(s => s.Resource.Id)
                .Must(s => s == default)
                .WithMessage("resource must be transient");
        }
    }
}
