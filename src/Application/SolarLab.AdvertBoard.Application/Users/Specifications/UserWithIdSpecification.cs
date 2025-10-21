using SolarLab.AdvertBoard.Domain.Users;
using SolarLab.AdvertBoard.SharedKernel.Specification;
using System.Linq.Expressions;

namespace SolarLab.AdvertBoard.Application.Users.Specifications
{
    /// <summary>
    /// Спецификация для фильтрации пользователей по идентификатору.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    public class UserWithIdSpecification(UserId userId) : Specification<User>
    {
        /// <inheritdoc/>
        public override Expression<Func<User, bool>> PredicateExpression => user => user.Id == userId;
    }
}
