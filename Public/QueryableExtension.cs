using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;

namespace ProductMange
{
    public static class QueryableExtension
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source,string sort,bool isAscending) where T:class
        {
            Type type = typeof(T);
            PropertyInfo propertyInfo = type.GetProperty(sort);
            if (propertyInfo == null)
            {
                throw new CustomExecption("9999", $"{sort}字段错误");
            }
            ParameterExpression param = Expression.Parameter(type, sort);
            Expression propertyAccessExpression = Expression.MakeMemberAccess(param, propertyInfo);
            LambdaExpression orderByExpression = Expression.Lambda(propertyAccessExpression, param);
            string method = isAscending ? "OrderBy" : "OrderByDescending";
            MethodCallExpression resultExpression = Expression.Call(typeof(Queryable), method, new Type[] {type,propertyInfo.PropertyType },source.Expression,Expression.Quote(orderByExpression));

            return source.Provider.CreateQuery<T>(resultExpression);
        }
    }
}
