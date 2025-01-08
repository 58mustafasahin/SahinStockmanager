using FluentValidation;
using SM.AuthorizationMgmt.Business.Features.Authorizations.Commands;
using SM.Core.Common.Constants;

namespace SM.AuthorizationMgmt.Business.Features.Authorizations.Validations
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("{PropertyName} " + Messages.CannotBeEmpty)
                .Length(2, 100).WithMessage("{PropertyName} " + string.Format(Messages.MustBeBetweenCharacter, "{MinLength}", "{MaxLength}"));

            RuleFor(x => x.CitizenId)
                .NotEmpty().WithMessage("{PropertyName} " + Messages.CannotBeEmpty)
                .Must(Be11Digits).WithMessage("{PropertyName} " + string.Format(Messages.MustBeCharacter, "11"));
            RuleFor(x => x.CitizenId)
                .Must(ValidateCitizenId).WithMessage("{PropertyName} " + Messages.IsNotValid)
                .When(x => Be11Digits(x.CitizenId));

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("{PropertyName} " + Messages.CannotBeEmpty)
                .Length(2, 100).WithMessage("{PropertyName} " + string.Format(Messages.MustBeBetweenCharacter, "{MinLength}", "{MaxLength}"));
            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("{PropertyName} " + Messages.CannotBeEmpty)
                .Length(2, 100).WithMessage("{PropertyName} " + string.Format(Messages.MustBeBetweenCharacter, "{MinLength}", "{MaxLength}"));
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("{PropertyName} " + Messages.CannotBeEmpty)
                .EmailAddress().WithMessage("{PropertyName} " + Messages.IsNotValid);
            RuleFor(x => x.MobilePhone)
                .NotEmpty().WithMessage("{PropertyName} " + Messages.CannotBeEmpty)
                .Matches(@"^\d{10}$").WithMessage("{PropertyName} " + Messages.IsNotValid);
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("{PropertyName} " + Messages.CannotBeEmpty)
                .Equal(x => x.ConfirmPassword).WithMessage(Messages.PasswordsDoNotMatch);
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("{PropertyName} " + Messages.CannotBeEmpty);
        }

        // Check if the number has exactly 11 digits
        private bool Be11Digits(long citizenId)
        {
            return citizenId.ToString().Length == 11;
        }

        // Custom validation logic
        private bool ValidateCitizenId(long citizenId)
        {
            string citizenIdStr = citizenId.ToString();

            // Convert to array of digits
            int[] digits = citizenIdStr.Select(digit => int.Parse(digit.ToString())).ToArray();

            // Calculate the sum of the first 10 digits
            int sum = digits.Take(10).Sum();

            // Check if the sum mod 11 equals the last digit
            return sum % 10 == digits[10];
        }
    }
}
