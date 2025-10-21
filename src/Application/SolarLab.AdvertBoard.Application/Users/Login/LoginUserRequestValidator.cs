using FluentValidation;
using SolarLab.AdvertBoard.Contracts.Authentication;

namespace SolarLab.AdvertBoard.Application.Users.Login
{
    /// <summary>
    /// Валидатор запроса на аутентификацию пользователя.
    /// </summary>
    public class LoginUserRequestValidator : AbstractValidator<LoginUserRequest>
    {
        /// <summary>
        /// Инициализирует правила валидации.
        /// </summary>
        public LoginUserRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required");
        }
    }
}
