using FluentValidation;
using IdS4.Application.Commands;

namespace IdS4.Application.Validations
{
    public class RemoveIdentityResourceCommandValidator
        :AbstractValidator<RemoveIdentityResourceCommand>
    {
        public RemoveIdentityResourceCommandValidator()
        {
            RuleFor(s => s.ResourceIds)
                .NotEmpty().WithMessage("resource ids is empty");
        }
    }
}
