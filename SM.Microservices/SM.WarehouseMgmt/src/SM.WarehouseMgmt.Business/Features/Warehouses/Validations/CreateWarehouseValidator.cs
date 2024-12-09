using FluentValidation;
using SM.Core.Common.Constants;
using SM.WarehouseMgmt.Business.Features.Warehouses.Commands;

namespace SM.WarehouseMgmt.Business.Features.Warehouses.Validations
{
    public class CreateWarehouseValidator : AbstractValidator<CreateWarehouseCommand>
    {
        public CreateWarehouseValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(x => "{PropertyName} " + Messages.CannotBeEmpty)
                .Length(2, 100).WithMessage("{PropertyName} " + string.Format(Messages.MustBeBetweenCharacter, "{MinLength}", "{MaxLength}"));
            RuleFor(x => x.Address)
                .NotEmpty().WithMessage(x => "{PropertyName} " + Messages.CannotBeEmpty)
                .Length(2, 500).WithMessage("{PropertyName} " + string.Format(Messages.MustBeBetweenCharacter, "{MinLength}", "{MaxLength}"));
            RuleFor(x => x.ResponsiblePerson)
                .NotEmpty().WithMessage(x => "{PropertyName} " + Messages.CannotBeEmpty)
                .Length(2, 100).WithMessage("{PropertyName} " + string.Format(Messages.MustBeBetweenCharacter, "{MinLength}", "{MaxLength}"));
        }
    }
}
