using System.Linq.Expressions;

namespace SolarLab.AdvertBoard.SharedKernel.Specification.Internal
{
    public class ExpressionRebinder<TSource, TTarget> : ExpressionVisitor
       where TTarget : TSource
    {
        private readonly ParameterExpression _newParameter;

        public ExpressionRebinder(ParameterExpression newParameter)
        {
            _newParameter = newParameter;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            // заменяем параметр исходного типа на новый
            if (node.Type == typeof(TSource))
                return _newParameter;

            return base.VisitParameter(node);
        }
    }
}