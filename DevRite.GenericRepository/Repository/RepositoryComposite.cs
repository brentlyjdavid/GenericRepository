using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Principal;
using System.Threading.Tasks;
using DevRite.GenericRepository.Interfaces;

#if NETSTANDARD
using Microsoft.EntityFrameworkCore;
#else
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
#endif

namespace DevRite.GenericRepository.Repository
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TUserKey"></typeparam>
    public class Repository<TEntity, TUserKey> : RepoBaseShared<TEntity, TUserKey>, IRepositoryCompositKeys<TEntity, TUserKey>
        where TEntity : class
        where TUserKey : IEquatable<TUserKey>
    {
        /// <summary>
        /// 
        /// </summary>
        public List<string> PrimaryKeys { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="currentUser"></param>
        /// <param name="isHangfire"></param>
        public Repository(DbContext ctx, IPrincipal currentUser, bool isHangfire) : base(ctx, currentUser, isHangfire)
        {
            GetKeyFieldNames();
        }

        /// <summary>
        /// Gets an entity by its primary key
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public TEntity GetById(List<object> keys, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            return GetByIdQueryable(keys, includes).FirstOrDefault();
        }
        /// <summary>
        /// Deletes an entity (Applies Updates based on interfaces)
        /// </summary>
        /// <param name="keys"></param>
        public void Delete(List<object> keys)
        {
            Delete(GetById(keys));
        }
        /// <summary>
        /// Deletes an entity (Applies Updates based on interfaces)
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public void DeleteAndSave(List<object> keys)
        {
            Delete(GetById(keys));
            Save();
        }
        /// <summary>
        /// Gets an entity by primary key
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public IQueryable<TEntity> GetByIdQueryable(List<object> keys, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            var query = AsQueryable(includes);
            for (var i = 0; i < PrimaryKeys.Count; i++)
            {
                ParameterExpression input = Expression.Parameter(typeof(TEntity));
#if NETSTANDARD
                var expLeft = Expression.Property(input, typeof(TEntity).GetTypeInfo().GetProperty(PrimaryKeys[i]));
#endif
#if !NETSTANDARD
                var expLeft = Expression.Property(input, typeof(TEntity).GetProperty(PrimaryKeys[i]));
#endif
                var expRight = Expression.Constant(keys[i]);
                query = query.Where(Expression.Lambda<Func<TEntity, bool>>(Expression.Equal(expLeft, expRight), input));
            }
            return query;
        }
        /// <summary>
        /// Gets an entity by its primary key
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="includes"></param>
        /// <returns><see cref="Task{TEntity}"/></returns>
        public Task<TEntity> GetByIdAsync(List<object> keys, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            return GetByIdQueryable(keys, includes).FirstOrDefaultAsync();
        }
        /// <summary>
        /// Deletes an entity (Applies Updates based on interfaces)
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        /// <exception cref="DbUpdateException">An error occurred sending updates to the database.</exception>
        /// <exception cref="DbUpdateConcurrencyException">
        ///             A database command did not affect the expected number of rows. This usually indicates an optimistic 
        ///             concurrency violation; that is, a row has been changed in the database since it was queried.
        ///             </exception>
        public Task DeleteAndSaveAsync(List<object> keys)
        {
            Delete(GetById(keys));
            return SaveAsync();
        }
        /// <summary>
        /// returns if entity exists by id (ignores constraints, this checks all items in the table)
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public bool Exists(List<object> keys)
        {
            return GetByIdQueryable(keys).Any();
        }
        /// <summary>
        /// returns if entity exists by id (ignores constraints, this checks all items in the table)
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public Task<bool> ExistsAsync(List<object> keys)
        {
            return GetByIdQueryable(keys).AnyAsync();
        }



        #region Helpers


#if NETSTANDARD
        private void GetKeyFieldNames()
        {
            var entityType = _context.Model.FindEntityType(typeof(TEntity));
            var key = entityType.FindPrimaryKey();
            PrimaryKeys = key.Properties.Select(i => i.Name).ToList();
        }
#endif

#if !NETSTANDARD
        private void GetKeyFieldNames() {
            ObjectContext objectContext = ((IObjectContextAdapter)_context).ObjectContext;
            ObjectSet<TEntity> set = objectContext.CreateObjectSet<TEntity>();
            IEnumerable<string> keyNames = set.EntitySet.ElementType
                                                        .KeyMembers
                                                        .Select(k => k.Name);
            PrimaryKeys = keyNames.ToList();
        }
#endif

        private List<object> GetKeyValues(TEntity entity)
        {
#if NETSTANDARD
            return PrimaryKeys.Select(primaryKey => typeof(TEntity).GetTypeInfo().GetProperty(primaryKey).GetValue(entity)).ToList();
#endif
#if !NETSTANDARD
            return PrimaryKeys.Select(primaryKey => typeof(TEntity).GetProperty(primaryKey).GetValue(entity)).ToList();
#endif
        }

        #endregion

        internal override void SetPrimaryKey(TEntity entity)
        {

        }

        internal override void AddOrUpdateLogic(TEntity entity)
        {
            base.AddOrUpdateLogic(entity);
            if (Exists(GetKeyValues(entity)))
            {
#if NETSTANDARD
                DbSet.Update(entity);
#else
                _context.Entry(entity).State = EntityState.Modified;
#endif
            }
            else
            {
                DbSet.Add(entity);
            }
        }
    }
}
