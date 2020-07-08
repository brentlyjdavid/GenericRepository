using System;
using System.Reflection;
using System.Security.Principal;
using DevRite.GenericRepository.Core.Interfaces;
#if NETSTANDARD
using Microsoft.EntityFrameworkCore;
#else
using System.Data.Entity;
#endif

namespace DevRite.GenericRepository.Repository
{
    /// <summary>
    /// default implementation of repository
    /// </summary>
    /// <typeparam name="TKey">type used for primary key</typeparam>
    /// <typeparam name="TEntity">the entity for this repository</typeparam>
    /// <typeparam name="TUserKey">the type used for the user primary key</typeparam>
    public class Repository<TKey, TEntity, TUserKey> : RepoShared<TKey, TEntity, TUserKey>
        where TKey : IEquatable<TKey>
        where TEntity : class, ISaveable<TKey>
        where TUserKey : IEquatable<TUserKey>
    {

        //private readonly string[] _keyNames;
        /// <summary>
        /// Default constructor for repository
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="currentUser"></param>
        /// <param name="isHangfire"></param>
        public Repository(DbContext entities, IPrincipal currentUser, bool isHangfire) : base(entities, currentUser, isHangfire) { }





        /// <summary>
        /// We assume change tracking at this point by EF
        /// </summary>
        /// <param name="updatingEntity"></param>
        internal override void AddOrUpdateLogic(TEntity updatingEntity)
        {
            base.AddOrUpdateLogic(updatingEntity);

            if (Exists(updatingEntity.Id))
            {
#if NETSTANDARD
DbSet.Update(updatingEntity);
#else
                _context.Entry(updatingEntity).State = EntityState.Modified;
#endif
            }
            else
            {
                DbSet.Add(updatingEntity);
            }
        }

        internal override void SetPrimaryKey(TEntity entity)
        {
            var id = entity.Id;
            if (typeof(TKey) == typeof(string))
            {
                var stringId = (string)Convert.ChangeType(id, typeof(string));
                if (String.IsNullOrEmpty(stringId))
                {
#if NETSTANDARD
                    entity.GetType().GetTypeInfo().GetProperty("Id").SetValue(entity, Guid.NewGuid().ToString());
#else
                    entity.GetType().GetProperty("Id").SetValue(entity, Guid.NewGuid().ToString());
#endif
                }
            }
        }
    }
}
