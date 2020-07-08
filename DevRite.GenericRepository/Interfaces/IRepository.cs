using System;
using System.Linq;
using System.Threading.Tasks;
using DevRite.GenericRepository.Core.Interfaces;

namespace DevRite.GenericRepository.Interfaces {
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TUserKey"></typeparam>
    public interface IRepository<in TKey, TEntity, TUserKey> : IRepositoryShared<TEntity>
        where TKey : IEquatable<TKey>
        where TEntity : class, ISaveable<TKey>
        where TUserKey : IEquatable<TUserKey> {
        
        /// <summary>
        /// Gets an entity by its primary key (applies constraints based on interfaces)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        TEntity GetById(TKey id, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);

        /// <summary>
        /// Gets an entity by primary key
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        IQueryable<TEntity> GetByIdQueryable(TKey id, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);

        /// <summary>
        /// Gets an entity by its primary key (applies constraints based on interfaces)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includes"></param>
        /// <returns><see cref="Task{TEntity}"/></returns>
        Task<TEntity> GetByIdAsync(TKey id, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);

        /// <summary>
        /// returns if entity exists by id (ignores constraints, this checks all items in the table)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Exists(TKey id);
        /// <summary>
        /// Returns if entity exists by id (ignores constraints, this checks all items in the table)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(TKey id);
       
        /// <summary>
        /// Deletes an entity (Applies Updates based on interfaces)
        /// </summary>
        /// <param name="id"></param>
        void Delete(TKey id);
        
        /// <summary>
        /// Deletes an entity (Applies Updates based on interfaces)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void DeleteAndSave(TKey id);

        /// <summary>
        /// Deletes an entity (Applies Updates based on interfaces)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAndSaveAsync(TKey id);
       
        
    }
}
