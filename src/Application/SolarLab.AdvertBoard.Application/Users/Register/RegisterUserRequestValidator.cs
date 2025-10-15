using FluentValidation;
using SolarLab.AdvertBoard.Application.Extensions;
using SolarLab.AdvertBoard.Contracts.Authentication;
using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.Domain.Users;

namespace SolarLab.AdvertBoard.Application.Users.Register
{
    public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
    {
        public RegisterUserRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(320).WithMessage("Email must not exceed 320 characters");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters")
                .MaximumLength(100).WithMessage("Password must not exceed 100 characters")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter")
                .Matches(@"\d").WithMessage("Password must contain at least one digit")
                .Matches(@"[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]").WithMessage("Password must contain at least one special character");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage(UserErrors.FirstName.Empty.Description)
                .MaximumLength(FirstName.MaxLength).WithMessage(UserErrors.FirstName.TooLong.Description)
                .Matches(FirstName.NameRegex()).WithMessage(UserErrors.FirstName.NotValid.Description);

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage(UserErrors.LastName.Empty.Description)
                .MaximumLength(LastName.MaxLength).WithMessage(UserErrors.LastName.TooLong.Description)
                .Matches(FirstName.NameRegex()).WithMessage(UserErrors.LastName.NotValid.Description);

            RuleFor(x => x.MiddleName)
                .MaximumLength(MiddleName.MaxLength).WithMessage(UserErrors.MiddleName.Empty.Description)
                .Matches(FirstName.NameRegex()).WithMessage(UserErrors.MiddleName.NotValid.Description)
                .When(x => !string.IsNullOrWhiteSpace(x.MiddleName));

            RuleFor(x => x.ContactEmail)
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(320).WithMessage("Contact email must not exceed 320 characters")
                .When(x => !string.IsNullOrWhiteSpace(x.ContactEmail));

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(PhoneNumber.MaxLength).WithMessage(UserErrors.PhoneNumber.TooLong.Description)
                .Matches(PhoneNumber.PhoneNumberRegex()).WithMessage(UserErrors.PhoneNumber.NotValid.Description)
                .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));
        }
    }
}
