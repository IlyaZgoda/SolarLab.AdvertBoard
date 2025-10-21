using System.Linq.Expressions;

namespace SolarLab.AdvertBoard.SharedKernel.Specification.Internal
{
    /// <summary>
    /// Посетитель выражений для перепривязки параметров выражений от типа <typeparamref name="TSource"/> к <typeparamref name="TTarget"/>.
    /// </summary>
    /// <typeparam name="TSource">Исходный тип параметра в выражении.</typeparam>
    /// <typeparam name="TTarget">Целевой тип параметра, должен быть наследником <typeparamref name="TSource"/>.</typeparam>
    public class ExpressionRebinder<TSource, TTarget> : ExpressionVisitor
       where TTarget : TSource
    {
        private readonly ParameterExpression _newParameter;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ExpressionRebinder{TSource, TTarget}"/>.
        /// </summary>
        /// <param name="newParameter">Новый параметр выражения с типом <typeparamref name="TTarget"/>.</param>
        public ExpressionRebinder(ParameterExpression newParameter)
        {
            _newParameter = newParameter;
        }


        /// <summary>
        /// Заменяет параметры типа <typeparamref name="TSource"/> на новый параметр типа <typeparamref name="TTarget"/>.
        /// </summary>
        /// <param name="node">Посещаемый параметр выражения.</param>
        /// <returns>
        /// Новый параметр <see cref="ParameterExpression"/> типа <typeparamref name="TTarget"/>, если исходный параметр имеет тип <typeparamref name="TSource"/>;
        /// иначе исходный параметр без изменений.
        /// </returns>
        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (node.Type == typeof(TSource))
                return _newParameter;

            return base.VisitParameter(node);
        }
    }
}