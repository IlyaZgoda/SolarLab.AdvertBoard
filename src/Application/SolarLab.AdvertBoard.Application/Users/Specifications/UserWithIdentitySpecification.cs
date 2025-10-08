using SolarLab.AdvertBoard.Domain.Users;
using SolarLab.AdvertBoard.SharedKernel.Specification;
using System.Linq.Expressions;

namespace SolarLab.AdvertBoard.Application.Users.Specifications
{
    public class UserWithIdentitySpecification(string identityId) : Specification<User>
    {
        public override Expression<Func<User, bool>> PredicateExpression => user => user.IdentityId == identityId;
    }

    public class UserWithIdSpecification(UserId userId) : Specification<User>
    {
        public override Expression<Func<User, bool>> PredicateExpression => user => user.Id == userId;
    }
}
