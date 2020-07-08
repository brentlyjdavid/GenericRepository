#if !NETSTANDARD
using System.Data.Entity;
#endif

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
#if  NETSTANDARD

#endif

#if !NETSTANDARD
using System.Data.Entity.Infrastructure;
#endif
namespace DevRite.GenericRepository.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepositoryShared<TEntity> : IView<TEntity>
        where TEntity : class
    {


        /// <summary>
        /// Allows an app to apply save logic to an entity
        /// </summary>
        Func<TEntity, TEntity> ApplyAdditionalSaveLogic { get; set; }
      

        /// <summary>
        /// Deletes an entity (Applies Updates based on interfaces)
        /// </summary>
        /// <param name="entity"></param>
        void Delete(TEntity entity);
        /// <summary>
        /// Deletes entities (Applies Updates based on interfaces)
        /// </summary>
        /// <param name="entities"></param>
        void Delete(List<TEntity> entities);
        /// <summary>
        /// Deletes an entity (Applies Updates based on interfaces)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        void DeleteAndSave(TEntity entity);
        /// <summary>
        /// Deletes an entity (Applies Updates based on interfaces)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task DeleteAndSaveAsync(TEntity entity);

        /// <summary>
        /// Deletes entities (Applies Updates based on interfaces)
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        void DeleteAndSave(List<TEntity> entities);
        /// <summary>
        /// Deletes entities (Applies Updates based on interfaces)
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task DeleteAndSaveAsync(List<TEntity> entities);

        /// <summary>
        /// Adds or updates entities based on their interfaces, and saves them to the table.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity AddOrUpdateAndSave(TEntity entity);
        /// <summary>
        /// Adds or updates entities based on their interfaces.
        /// Method assumes entity being passed in already has been tracked by Entity Framework
        /// </summary>
        /// <param name="entity"></param>
        void AddOrUpdate(TEntity entity);
        /// <summary>
        /// Adds or updates entities based on their interfaces, and saves them to the table.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<TEntity> AddOrUpdateAndSaveAsync(TEntity entity);
    }
}
