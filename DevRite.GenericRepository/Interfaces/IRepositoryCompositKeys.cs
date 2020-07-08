using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevRite.GenericRepository.Interfaces {
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TUserKey"></typeparam>
    public interface IRepositoryCompositKeys<TEntity, TUserKey> : IRepositoryShared<TEntity>
        where TEntity : class
        where TUserKey : IEquatable<TUserKey> {
        /// <summary>
        /// Gets an entity by its primary key
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        TEntity GetById(List<object> keys , Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);

        /// <summary>
        /// Gets an entity by primary key
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        IQueryable<TEntity> GetByIdQueryable(List<object> keys, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);


        /// <summary>
        /// Gets an entity by its primary key
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="includes"></param>
        /// <returns><see cref="Task{TEntity}"/></returns>
        Task<TEntity> GetByIdAsync(List<object> keys, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);
        /// <summary>
        /// Deletes an entity (Applies Updates based on interfaces)
        /// </summary>
        /// <param name="keys"></param>
        void Delete(List<object> keys);

        /// <summary>
        /// Deletes an entity (Applies Updates based on interfaces)
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        void DeleteAndSave(List<object> keys);

        /// <summary>
        /// Deletes an entity (Applies Updates based on interfaces)
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task DeleteAndSaveAsync(List<object> keys);

        /// <summary>
        /// returns if entity exists by id (ignores constraints, this checks all items in the table)
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        bool Exists(List<object> keys);
        /// <summary>
        /// Returns if entity exists by id (ignores constraints, this checks all items in the table)
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(List<object> keys);
    }
}
