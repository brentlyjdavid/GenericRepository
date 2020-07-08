using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DevRite.GenericRepository.Core.Interfaces;

namespace DevRite.GenericRepository.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class IQueryableExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="currentQuery"></param>
        /// <returns></returns>
        public static IQueryable<TEntity> ApplyRepositoryConstraints<TEntity>(this IQueryable<TEntity> currentQuery)
        {
#if NETSTANDARD
            var interfaces = typeof(TEntity).GetTypeInfo().GetInterfaces();
#endif
#if !NETSTANDARD
            var interfaces = typeof(TEntity).GetInterfaces();
#endif
            if (interfaces.Contains(typeof(ISaveableDelete)))
            {
                var info = typeof(TEntity).GetTypeInfo().GetProperty("DateDeletedUtc");
                var lamdaArg = Expression.Parameter(typeof(TEntity));
                var propertyAccess = Expression.MakeMemberAccess(lamdaArg, info);
                var propertyEquals = Expression.Equal(propertyAccess, Expression.Constant(null, typeof(DateTime?)));
                var expressionHere = Expression.Lambda<Func<TEntity, bool>>(propertyEquals, lamdaArg);
                currentQuery = currentQuery.Where(expressionHere);
            }

            if (interfaces.Contains(typeof(ISaveableActive)) && !interfaces.Contains(typeof(ISaveableDelete)))
            {
                var info = typeof(TEntity).GetTypeInfo().GetProperty("IsActive");
                var lamdaArg = Expression.Parameter(typeof(TEntity));
                var propertyAccess = Expression.MakeMemberAccess(lamdaArg, info);
                var propertyEquals = Expression.Equal(propertyAccess, Expression.Constant(true, typeof(bool)));
                var expressionHere = Expression.Lambda<Func<TEntity, bool>>(propertyEquals, lamdaArg);
                currentQuery = currentQuery.Where(expressionHere);
            }

            if (interfaces.Contains(typeof(IOrderable)) && !interfaces.Contains(typeof(IOrderableDescending)))
            {
                var type = typeof(TEntity);
                var property = type.GetTypeInfo().GetProperty("DisplayOrder");
                var parameter = Expression.Parameter(type, "p");
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                var typeArguments = new Type[] { type, property.PropertyType };
                var methodName = "OrderBy";
                var resultExp = Expression.Call(typeof(Queryable), methodName, typeArguments, currentQuery.Expression, Expression.Quote(orderByExp));
                return currentQuery.Provider.CreateQuery<TEntity>(resultExp);
            }

            if (interfaces.Contains(typeof(IOrderableDescending)) && !interfaces.Contains(typeof(IOrderable)))
            {
                var type = typeof(TEntity);
                var property = type.GetTypeInfo().GetProperty("DisplayOrder");
                var parameter = Expression.Parameter(type, "p");
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                var typeArguments = new Type[] { type, property.PropertyType };
                var methodName = "OrderByDescending";
                var resultExp = Expression.Call(typeof(Queryable), methodName, typeArguments, currentQuery.Expression, Expression.Quote(orderByExp));
                return currentQuery.Provider.CreateQuery<TEntity>(resultExp);
            }

            return currentQuery;
        }
    }
}
