using FluentValidation;
using SM.Core.Common.Constants;
using SM.ProductMgmt.Business.Features.Categories.Commands;

namespace SM.ProductMgmt.Business.Features.Categories.Validations
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage(x => "{PropertyName} " + Messages.CannotBeEmpty);
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(x => "{PropertyName} " + Messages.CannotBeEmpty)
                .Length(2, 100).WithMessage("{PropertyName} " + string.Format(Messages.MustBeBetweenCharacter, "{MinLength}", "{MaxLength}"));
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage(x => "{PropertyName} " + Messages.CannotBeEmpty)
                .Length(2, 500).WithMessage("{PropertyName} " + string.Format(Messages.MustBeBetweenCharacter, "{MinLength}", "{MaxLength}"));
        }
    }
}
