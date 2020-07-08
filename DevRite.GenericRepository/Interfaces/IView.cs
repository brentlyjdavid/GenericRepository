using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Threading.Tasks;
using DevRite.GenericRepository.Core.Interfaces;
#if NETSTANDARD

#else
using System.Data.Entity;
#endif


namespace DevRite.GenericRepository.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IView<TEntity>
    where TEntity : class
    {
        /// <summary>
        /// Applies your own constraints on every query.  Best used on multi-tenant projects.
        /// </summary>
        Func<IQueryable<TEntity>, IQueryable<TEntity>> ApplyAdditionalConstraints { get; set; }

        /// <summary>
        /// Applies additional constraints to every query. This method should NOT contain user based or claims based constraints
        /// </summary>
        Func<IQueryable<TEntity>, IQueryable<TEntity>> ApplyAdditionalHangfireOnlyConstraints { get; set; }


        /// <summary>
        /// Applies more constraints based on ClaimsPrincipal.Current or _currentUser.  This method will ONLY be called if _currentUser is not null and _currentUser.Identity.IsAuthenticated is true
        /// </summary>
        Func<IQueryable<TEntity>, IPrincipal, IQueryable<TEntity>> ApplyAdditionalUserClaimsOnlyConstraints { get; set; }



        /// <summary>
        /// Gets <see cref="IQueryable{TEntity}"/> (applies constraints based on interfaces)
        /// </summary>
        /// <param name="includes"></param>
        /// <returns></returns>
        IQueryable<TEntity> AsQueryable(Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);

        /// <summary>
        /// This function returns a queryable of objects with the Interfaces applied.  i.e. if your object has <see cref="ISaveableDelete"/> and <see cref="ISaveableActive"/>, this method will return all objects who are not deleted, but ignores active.  if your object has <see cref="ISaveableActive"/> only, it returns objects that have the active flag set to <code>true</code>
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);

        /// <summary>
        /// Gets an entity based on specified parameters, will error out if more than one are found. (applies constraints based on interfaces)
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includes"></param>
        /// <returns><see cref="List{TEntity}"/></returns>
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);
        /// <summary>
        /// Gets an entity based on specified parameters, will error out if more than one are found. (applies constraints based on interfaces)
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includes"></param>
        /// <returns><see cref="List{TEntity}"/></returns>
        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);

        /// <summary>
        /// Gets the first found entity based on specified parameters (applies constraints based on interfaces)
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includes"></param>
        /// <returns><see cref="List{TEntity}"/></returns>
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);
        /// <summary>
        /// Gets the first found entity based on specified parameters (applies constraints based on interfaces)
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includes"></param>
        /// <returns><see cref="List{TEntity}"/></returns>
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);

        /// <summary>
        /// Gets all entities (applies constraints based on interfaces)
        /// </summary>
        /// <param name="includes"></param>
        /// <returns><see cref="List{TEntity}"/></returns>
        List<TEntity> ToList(Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);
        /// <summary>
        /// Gets all entities (applies constraints based on interfaces)
        /// </summary>
        /// <param name="includes"></param>
        /// <returns><see cref="List{TEntity}"/></returns>
        Task<List<TEntity>> ToListAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);
    }
}
