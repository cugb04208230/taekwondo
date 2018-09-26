using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Common.Data;

namespace Common.Extensions
{
    public static class QueryableExtensions
    {

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, String propertyName)
        {
            return InternalOrder<T>(source, propertyName, "OrderBy");
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, String propertyName)
        {
            return InternalOrder<T>(source, propertyName, "OrderByDescending");
        }

        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, String propertyName)
        {
            return InternalOrder<T>(source, propertyName, "ThenBy");
        }
        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, String propertyName)
        {
            return InternalOrder<T>(source, propertyName, "ThenByDescending");
        }

        public static IOrderedQueryable<T> InternalOrder<T>(this IQueryable<T> source, String propertyName, String methodName)
        {
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "p");
            PropertyInfo property = type.GetProperty(propertyName);
            Expression expr = Expression.Property(arg, property);
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), property.PropertyType);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

            return ((IOrderedQueryable<T>)(typeof(Queryable).GetMethods().Single(
                    p => String.Equals(p.Name, methodName, StringComparison.Ordinal)
                         && p.IsGenericMethodDefinition
                         && p.GetGenericArguments().Length == 2
                         && p.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), property.PropertyType)
                .Invoke(null, new Object[] { source, lambda })));
        }


        public static IQueryable<T> InternalOrder<T>(this IQueryable<T> source, List<OrderField> orders)
        {
            for (int i = 0; i < orders.Count; i++)
            {
                var order = orders[i];
                var methodName = (i == 0 ? "OrderBy" : "ThenBy") + 
                    (order.OrderDirection == OrderDirection.Desc? "Descending": "");
                source = source.InternalOrder(order.OrderBy, methodName);
            }
            return source;
        }


        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="orders"></param>
        /// <returns></returns>
        public static IQueryable<T> InternalOrderBy<T>(this IQueryable<T> queryable, List<OrderField> orders)
        {
            if (orders == null || orders.Count == 0)
            {
                orders=new List<OrderField>{new OrderField{OrderBy = "Id",OrderDirection = OrderDirection.Desc}};
            }
            return queryable.InternalOrder(orders);
        }
    }
}
