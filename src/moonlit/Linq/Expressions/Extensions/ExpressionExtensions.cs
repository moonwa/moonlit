using System;
using System.Linq.Expressions;

namespace Moonlit.Linq.Expressions
{
    public static class ExpressionExtensions
    {
        public static string GetMemberName<TModel, TValue>(this Expression<Func<TModel, TValue>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression != null)
                return memberExpression.Member.Name;
            return string.Empty;
        }
    }
}
