using FluentValidation;
using SolarLab.AdvertBoard.Application.Extensions;
using SolarLab.AdvertBoard.Contracts.Authentication;

namespace SolarLab.AdvertBoard.Application.Users.Login
{
    public sealed class LoginUserRequestValidator : AbstractValidator<LoginUserRequest>
    {
        public LoginUserRequestValidator()
        {
            RuleFor(x => x.Email)
                .ApplyEmailValidation();

            RuleFor(x => x.Password)
                .ApplyPasswordValidation();

        }
    }
}
