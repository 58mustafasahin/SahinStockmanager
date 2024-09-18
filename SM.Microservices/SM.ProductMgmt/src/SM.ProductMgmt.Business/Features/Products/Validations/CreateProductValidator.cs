using FluentValidation;
using SM.Core.Common.Constants;
using SM.ProductMgmt.Business.Features.Products.Commands;

namespace SM.ProductMgmt.Business.Features.Products.Validations
{
    public class CreateProductValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(x => "{PropertyName} " + Messages.CannotBeEmpty)
                .Length(2, 100).WithMessage("{PropertyName} " + string.Format(Messages.MustBeBetweenCharacter, "{MinLength}", "{MaxLength}"));
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage(x => "{PropertyName} " + Messages.CannotBeEmpty)
                .Length(2, 500).WithMessage("{PropertyName} " + string.Format(Messages.MustBeBetweenCharacter, "{MinLength}", "{MaxLength}"));
            RuleFor(x => x.Price).NotEmpty().WithMessage(x => "{PropertyName} " + Messages.CannotBeEmpty);
            RuleFor(x => x.StockQuantity).NotEmpty().WithMessage(x => "{PropertyName} " + Messages.CannotBeEmpty);
            RuleFor(x => x.Unit).NotEmpty().WithMessage(x => "{PropertyName} " + Messages.CannotBeEmpty);
        }
    }
}
