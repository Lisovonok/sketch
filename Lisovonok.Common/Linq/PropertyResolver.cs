using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace Lisovonok.Common.Linq
{
    public static class PropertyResolver
    {
        public static string GetPropertyName<T>(this T type, Expression<Func<T, object>> expression)
        {
            return GetPropertyName<T>(expression);
        }

        public static string GetPropertyName<T>(Expression<Func<T, object>> expression)
        {
            var lambda = expression as LambdaExpression;
            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = lambda.Body as UnaryExpression;
                memberExpression = unaryExpression.Operand as MemberExpression;
            }
            else
            {
                memberExpression = lambda.Body as MemberExpression;
            }

            if (memberExpression != null)
            {
                var propertyInfo = memberExpression.Member as PropertyInfo;
                return propertyInfo.Name;
            }
            return null;
        }
    }
}
