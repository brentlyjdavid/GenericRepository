using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using DevRite.Extensions;
using DevRite.GenericRepository.Core.Interfaces;
using DevRite.GenericRepository.Interfaces;
using DevRite.GenericRepository.View;

using IPrincipleExtensions = DevRite.GenericRepository.Helpers.IPrincipleExtensions;
#if  NETSTANDARD
using Microsoft.EntityFrameworkCore;
#else
using System.Data.Entity;
#endif

namespace DevRite.GenericRepository.Repository
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TUserKey"></typeparam>
    public abstract class RepoBaseShared<TEntity, TUserKey> : View<TEntity>, IRepositoryShared<TEntity>
        where TEntity : class
        where TUserKey : IEquatable<TUserKey>
    {
        /// <summary>
        /// 
        /// </summary>
        protected bool _hasCreatedByTracking { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected bool _hasUpdatedByTracking { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected bool _hasTracking { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected bool _hasStart { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected bool _hasEnd { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected bool _hasCreated { get; set; }

        /// <inheritdoc />
        public Func<TEntity, TEntity> ApplyAdditionalSaveLogic { get; set; }

        /// <inheritdoc cref="View{TEntity}(DbContext,System.Security.Principal.IPrincipal,bool,bool)"/>
        protected RepoBaseShared(DbContext ctx, IPrincipal user, bool isHangfire) : base(ctx, user, isHangfire, false)
        {
            Setup();
        }


        /// <inheritdoc cref="View{TEntity}(DbContext,bool,bool)"/>
        protected RepoBaseShared(DbContext ctx, bool isHangfire) : base(ctx, isHangfire, false)
        {
            Setup();
        }


        /// <inheritdoc cref="View{TEntity}(DbContext)"/>
        protected RepoBaseShared(DbContext ctx) : base(ctx)
        {
            Setup();
        }

        private void Setup()
        {
            var interfaces = typeof(TEntity).GetInterfaces();
            _hasStart = interfaces.Contains(typeof(ISaveableStart));
            _hasEnd = interfaces.Contains(typeof(ISaveableEnd));
            _hasCreatedByTracking = interfaces.Contains(typeof(ISaveableUserCreateTracking<TUserKey>));
            _hasUpdatedByTracking = interfaces.Contains(typeof(ISaveableUserUpdateTracking<TUserKey>));
            _hasTracking = interfaces.Contains(typeof(ISaveableTracking));
            _hasCreated = interfaces.Contains(typeof(ISaveableCreate));
        }





        /// <summary>
        /// Deletes an entity (Applies Updates based on interfaces)
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(TEntity entity)
        {
            Delete(new List<TEntity>() { entity });
        }

        /// <summary>
        /// Deletes entities (Applies Updates based on interfaces)
        /// </summary>
        /// <param name="entities"></param>
        public void Delete(List<TEntity> entities)
        {
            foreach (var dbEntity in entities)
            {
                if (_hasDateDeleted)
                {
#if NETSTANDARD
                    dbEntity.GetType().GetTypeInfo().GetProperty("DateDeletedUtc").SetValue(dbEntity, DateTime.UtcNow);
#endif

#if !NETSTANDARD
                    dbEntity.GetType().GetProperty("DateDeletedUtc").SetValue(dbEntity, DateTime.UtcNow);
#endif
                    AddOrUpdate(dbEntity);
                }
                else
                {
                    DbSet.Remove(dbEntity);
                }
            }
        }

        /// <summary>
        /// Deletes entities (Applies Updates based on interfaces)
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public void DeleteAndSave(List<TEntity> entities)
        {
            AsyncTask.RunSync(() => DeleteAndSaveAsync(entities));
        }

        /// <summary>
        /// Deletes entities (Applies Updates based on interfaces)
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public Task DeleteAndSaveAsync(List<TEntity> entities)
        {
            Delete(entities);
            return SaveAsync();
        }

        /// <inheritdoc cref="IRepositoryShared{TEntity}.DeleteAndSave(TEntity)"/>
        public void DeleteAndSave(TEntity entity)
        {
            AsyncTask.RunSync(() => DeleteAndSaveAsync(entity));
        }


        /// <summary>
        /// Deletes an entity (Applies Updates based on interfaces)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task DeleteAndSaveAsync(TEntity entity)
        {
            Delete(entity);
            return SaveAsync();
        }


        /// <inheritdoc cref="IRepositoryShared{TEntity}.AddOrUpdateAndSave"/>
        public TEntity AddOrUpdateAndSave(TEntity entity)
        {
            AddOrUpdate(entity);
            Save();
            return entity;
        }

        ///<inheritdoc cref="IRepositoryShared{TEntity}.AddOrUpdate"/>
        public void AddOrUpdate(TEntity entity)
        {
            AddOrUpdateLogic(entity);
        }

        ///<inheritdoc cref="IRepositoryShared{TEntity}.AddOrUpdateAndSaveAsync"/>
        public async Task<TEntity> AddOrUpdateAndSaveAsync(TEntity entity)
        {
            AddOrUpdate(entity);
            await SaveAsync();
            return entity;
        }

        internal virtual void AddOrUpdateLogic(TEntity entity)
        {
            entity = ApplyUpdateConstraints(ref entity);
            entity = ApplyAdditionalSaveLogic?.Invoke(entity) ?? entity;
            SetPrimaryKey(entity);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected TEntity ApplyUpdateConstraints(ref TEntity entity)
        {
            if (entity == null) return entity;

            if (_hasTracking)
            {
#if NETSTANDARD
                entity.GetType().GetTypeInfo().GetProperty("DateLastModifiedUtc").SetValue(entity, DateTime.UtcNow);
#endif
#if !NETSTANDARD
                entity.GetType().GetProperty("DateLastModifiedUtc").SetValue(entity, DateTime.UtcNow);
#endif

            }

            if (_hasCreated)
            {
                var currentValue = (DateTime)entity.GetType().GetProperty("DateCreatedUtc").GetValue(entity, null);
                if (currentValue == DateTime.MinValue)
                {
#if NETSTANDARD
                    entity.GetType().GetTypeInfo().GetProperty("DateCreatedUtc").SetValue(entity, DateTime.UtcNow);
#endif

#if !NETSTANDARD
                entity.GetType().GetProperty("DateCreatedUtc").SetValue(entity, DateTime.UtcNow);
#endif
                }
            }


            UpdateCreatedById(ref entity);

            UpdateUpdatedById(ref entity);



            return entity;
        }

        private void UpdateCreatedById(ref TEntity entity)
        {
            if (!_hasCreatedByTracking) return;
            if (!(_currentUser?.Identity?.IsAuthenticated ?? false)) return;

            var userIdClaim = IPrincipleExtensions.GetClaimValue(_currentUser, ClaimTypes.NameIdentifier);
            if (String.IsNullOrEmpty(userIdClaim))
            {
                userIdClaim = IPrincipleExtensions.GetClaimValue(_currentUser, "sub"); //used in .net core
            }
#if NETSTANDARD
            var currentVal = entity.GetType().GetTypeInfo().GetProperty("CreatedById").GetValue(entity, null);
            if (currentVal == null && userIdClaim != null)
            {
                entity.GetType().GetTypeInfo().GetProperty("CreatedById").SetValue(entity, userIdClaim);
            }
#endif

#if !NETSTANDARD
            var currentVal = entity.GetType().GetProperty("CreatedById").GetValue(entity, null);
            if (currentVal == null && userIdClaim != null)
            {
                entity.GetType().GetProperty("CreatedById").SetValue(entity, userIdClaim);
            }
#endif
        }

        private void UpdateUpdatedById(ref TEntity entity)
        {
            if (!_hasUpdatedByTracking) return;
            if (!(_currentUser?.Identity?.IsAuthenticated ?? false)) return;

            var userIdClaim = IPrincipleExtensions.GetClaimValue(_currentUser, ClaimTypes.NameIdentifier);
            if (String.IsNullOrEmpty(userIdClaim))
            {
                userIdClaim = IPrincipleExtensions.GetClaimValue(_currentUser, "sub"); //used in .net core
            }
#if NETSTANDARD
            entity.GetType().GetTypeInfo().GetProperty("LastModifiedById").SetValue(entity, userIdClaim);
#endif
#if !NETSTANDARD
            entity.GetType().GetProperty("LastModifiedById").SetValue(entity, userIdClaim);
#endif
        }



        internal abstract void SetPrimaryKey(TEntity entity);
        internal bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        internal async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}
