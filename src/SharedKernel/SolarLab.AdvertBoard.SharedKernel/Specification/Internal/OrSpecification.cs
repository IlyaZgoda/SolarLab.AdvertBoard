using System.Linq.Expressions;
using SolarLab.AdvertBoard.SharedKernel.Specification;
using SolarLab.AdvertBoard.SharedKernel.Specification.Extensions;

namespace SolarLab.AdvertBoard.SharedKernel.Specification.Internal
{
    /// <summary>
    /// Спецификация "ИЛИ".
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности.</typeparam>
    internal class OrSpecification<TEntity> : Specification<TEntity>
    {
        /// <inheritdoc />
        public override Expression<Func<TEntity, bool>> PredicateExpression { get; }
        
        /// <summary>
        /// Инициализирует экземпляр спецификации <see cref="OrSpecification{TEntity}"/>.
        /// </summary>
        public OrSpecification(ISpecification<TEntity> left, ISpecification<TEntity> right)
        {
            if (left == null) throw new ArgumentNullException(nameof(left));
            if (right == null) throw new ArgumentNullException(nameof(right));

            PredicateExpression = left.PredicateExpression.Or(right.PredicateExpression);
        }
    }
}