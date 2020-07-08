using System;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using DevRite.Extensions;
using DevRite.GenericRepository.Core.Interfaces;
using DevRite.GenericRepository.Interfaces;

#if NETSTANDARD
using Microsoft.EntityFrameworkCore;
#else
using System.Data.Entity;
#endif

namespace DevRite.GenericRepository.Repository
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TUserKey"></typeparam>
    public abstract class RepoShared<TKey, TEntity, TUserKey> : RepoBaseShared<TEntity, TUserKey>, IRepository<TKey, TEntity, TUserKey>
        where TKey : IEquatable<TKey>
        where TEntity : class, ISaveable<TKey>
        where TUserKey : IEquatable<TUserKey>
    {
/// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="currentUser"></param>
        /// <param name="isHangfire"></param>
        protected RepoShared(DbContext entities, IPrincipal currentUser, bool isHangfire) : base(entities, currentUser, isHangfire) { }

/// <summary>
        /// Deletes an entity (Applies Updates based on interfaces)
        /// </summary>
        /// <param name="id"></param>
        public void Delete(TKey id)
        {
            Delete(GetById(id));
        }

        /// <summary>
        /// Deletes an entity (Applies Updates based on interfaces)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public void DeleteAndSave(TKey id)
        {
            AsyncTask.RunSync(() => DeleteAndSaveAsync(id));
        }

        
       /// <summary>
        /// Gets an entity by id. Ignores all pre-defined logic
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public TEntity GetById(TKey id, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            return AsyncTask.RunSync(() => GetByIdAsync(id, includes));
        }

        /// <summary>
        /// Gets an entity by id. Ignores all pre-defined logic
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public IQueryable<TEntity> GetByIdQueryable(TKey id, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            var query = _asNoTracking ? DbSet.AsNoTracking() : DbSet.AsQueryable();
            if (includes != null)
            {
                query = includes?.Invoke(query);
            }

            return query.Where(m => m.Id.Equals(id));
        }


        /// <summary>
        /// Gets an entity by id. Ignores all pre-defined logic
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public Task<TEntity> GetByIdAsync(TKey id, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            return GetByIdQueryable(id, includes).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Figures out if it exists, ignores all pre-defined logic
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Exists(TKey id)
        {
            return AsyncTask.RunSync(() => ExistsAsync(id));
        }

        /// <summary>
        /// Figures out if it exists, ignores all pre-defined logic
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<bool> ExistsAsync(TKey id)
        {
            return DbSet.AsNoTracking().AnyAsync(m => m.Id.Equals(id));
        }
        /// <summary>
        /// Deletes an entity (Applies Updates based on interfaces)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task DeleteAndSaveAsync(TKey id)
        {
            Delete(id);
            return SaveAsync();
        }

        
    }
}
