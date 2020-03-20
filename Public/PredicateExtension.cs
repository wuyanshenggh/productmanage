using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace ProductMange
{
    public static class PredicateExtension
    {
        public static Expression<Func<T,bool>> And<T>(this Expression<Func<T,bool>> expression1,Expression<Func<T,bool>> expression2)
        {
            var parameterExpression = Expression.Parameter(typeof(T), "param");
            var parameterReplacer = new ParameterReplacer(parameterExpression);
            var left = parameterReplacer.Visit(expression1.Body);
            var right = parameterReplacer.Visit(expression2.Body);
            var body = Expression.And(left, right);

            return Expression.Lambda<Func<T,bool>>(body,parameterExpression);
        }
    }

    internal class ParameterReplacer : ExpressionVisitor
    {
        public ParameterReplacer(ParameterExpression paramExpr)
        {
            this.ParameterExpression = paramExpr;
        }

        public ParameterExpression ParameterExpression { get; private set; }

        public Expression Replace(Expression expr)
        {
            return this.Visit(expr);
        }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            return this.ParameterExpression;
        }
    }
}
