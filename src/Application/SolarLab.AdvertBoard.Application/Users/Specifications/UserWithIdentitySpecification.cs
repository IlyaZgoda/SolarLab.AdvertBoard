using SolarLab.AdvertBoard.Domain.Users;
using SolarLab.AdvertBoard.SharedKernel.Specification;
using System.Linq.Expressions;

namespace SolarLab.AdvertBoard.Application.Users.Specifications
{
    /// <summary>
    /// Спецификция для фильтрации пользователей по идентификатору пользователя в системе аутентификации/>
    /// </summary>
    /// <param name="identityId">Идентификатор пользователя в системе аутентификации.</param>
    public class UserWithIdentitySpecification(string identityId) : Specification<User>
    {
        /// <inheritdoc/>
        public override Expression<Func<User, bool>> PredicateExpression => user => user.IdentityId == identityId;
    }
}
