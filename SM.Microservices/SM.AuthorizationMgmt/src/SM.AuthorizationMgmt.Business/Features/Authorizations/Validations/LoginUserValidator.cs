using FluentValidation;
using SM.AuthorizationMgmt.Business.Features.Authorizations.Commands;
using SM.Core.Common.Constants;

namespace SM.AuthorizationMgmt.Business.Features.Authorizations.Validations
{
    public class LoginUserValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage(x => "{PropertyName} " + Messages.CannotBeEmpty);
            RuleFor(x => x.Password).NotEmpty().WithMessage(x => "{PropertyName} " + Messages.CannotBeEmpty);
        }
    }
}
