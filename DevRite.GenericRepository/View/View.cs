using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Threading.Tasks;
using DevRite.Extensions;
using DevRite.GenericRepository.Core.Interfaces;
using DevRite.GenericRepository.Extensions;
using DevRite.GenericRepository.Interfaces;

#if NETSTANDARD
using Microsoft.EntityFrameworkCore;
#else
using System.Data.Entity;
#endif  

namespace DevRite.GenericRepository.View
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class View<TEntity> : IView<TEntity> where TEntity : class
    {
        /// <summary>
        /// 
        /// </summary>
        protected readonly DbContext _context;

        private bool _isReadOnly { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected bool _hasDateDeleted { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected bool _hasActive { get; set; }

        /// <summary>
        /// Whether or not this repository is used in hangfire (See other projects by Author)
        /// </summary>
        protected bool _isHangfire { get; set; }

        /// <summary>
        /// Current User is ASP.NET
        /// </summary>
        protected IPrincipal _currentUser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected bool _asNoTracking { get; set; }
#if NETSTANDARD
        /// <summary>
        /// 
        /// </summary>
        public virtual DbQuery<TEntity> DbQuery => _context.Query<TEntity>();
#endif
        /// <summary>
        /// 
        /// </summary>
        public virtual DbSet<TEntity> DbSet => _context.Set<TEntity>();


        /// <inheritdoc cref="IView{TEntity}.ApplyAdditionalConstraints"/>
        public Func<IQueryable<TEntity>, IQueryable<TEntity>> ApplyAdditionalConstraints { get; set; }

        /// <inheritdoc cref="IView{TEntity}.ApplyAdditionalHangfireOnlyConstraints"/>
        public Func<IQueryable<TEntity>, IQueryable<TEntity>> ApplyAdditionalHangfireOnlyConstraints { get; set; }

        /// <inheritdoc cref="IView{TEntity}.ApplyAdditionalUserClaimsOnlyConstraints"/>
        public Func<IQueryable<TEntity>, IPrincipal, IQueryable<TEntity>> ApplyAdditionalUserClaimsOnlyConstraints { get; set; }

        /// <summary>
        /// Context with ASP.NET principle, and hangfire
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="user"></param>
        /// <param name="isHangfire"></param>
        /// <param name="isReadOnly"></param>
        public View(DbContext ctx, IPrincipal user, bool isHangfire, bool isReadOnly = true) : this(ctx, isHangfire, isReadOnly)
        {
            _currentUser = user;
        }

        /// <summary>
        /// Context with hangfire
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="isHangfire"></param>
        /// <param name="isReadOnly"></param>
        public View(DbContext ctx, bool isHangfire, bool isReadOnly = true) : this(ctx)
        {
            _isHangfire = isHangfire;
            _isReadOnly = isReadOnly;
        }

        /// <summary>
        /// Context Only
        /// </summary>
        /// <param name="ctx"></param>
        public View(DbContext ctx)
        {
            _context = ctx;
            _isReadOnly = true;
            Setup();
        }


        /// <summary>
        /// Sets up class
        /// </summary>
        private void Setup()
        {
            var interfaces = typeof(TEntity).GetInterfaces();
            _hasDateDeleted = interfaces.Contains(typeof(ISaveableDelete));
            _hasActive = interfaces.Contains(typeof(ISaveableActive));
        }

        /// <summary>
        /// Function that Adds any additional items
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        protected IQueryable<TEntity> AdditionalConstraints(IQueryable<TEntity> query)
        {
            if (ApplyAdditionalConstraints != null)
            {
                query = ApplyAdditionalConstraints(query);
            }

            if (ApplyAdditionalHangfireOnlyConstraints != null && _isHangfire)
            {
                query = ApplyAdditionalHangfireOnlyConstraints(query);
            }

            if (ApplyAdditionalUserClaimsOnlyConstraints != null && _currentUser != null && _currentUser.Identity.IsAuthenticated && !_isHangfire)
            {
                query = ApplyAdditionalUserClaimsOnlyConstraints(query, _currentUser);
            }
            return query;
        }


        /// <inheritdoc cref="IView{TEntity}.AsQueryable"/>
        public IQueryable<TEntity> AsQueryable(Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            return Where(null, includes);
        }


        /// <inheritdoc cref="IView{TEntity}.Where"/>
        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
#if NETSTANDARD
            var query = _isReadOnly ? DbQuery.AsQueryable() : DbSet.AsQueryable();
#else
            var query = DbSet.AsQueryable();
#endif

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return AdditionalConstraints(ApplyActiveConstraints(GetByIncludes(query, includes)));
        }


        /// <inheritdoc cref="IView{TEntity}.AsQueryable"/>
        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            return AsyncTask.RunSync(() => SingleOrDefaultAsync(predicate, includes));
        }

        /// <inheritdoc cref="IView{TEntity}.AsQueryable"/>
        public Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            return Where(predicate, includes).SingleOrDefaultAsync();
        }

        /// <inheritdoc cref="IView{TEntity}.AsQueryable"/>
        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            return AsyncTask.RunSync(() => FirstOrDefaultAsync(predicate, includes));
        }

        /// <inheritdoc cref="IView{TEntity}.AsQueryable"/>
        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            return Where(predicate, includes).FirstOrDefaultAsync();
        }

        /// <inheritdoc cref="IView{TEntity}.AsQueryable"/>
        public List<TEntity> ToList(Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            return AsyncTask.RunSync(() => ToListAsync(includes));
        }

        /// <inheritdoc cref="IView{TEntity}.AsQueryable"/>
        public Task<List<TEntity>> ToListAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            return Where(null, includes).ToListAsync();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        protected IQueryable<TEntity> ApplyActiveConstraints(IQueryable<TEntity> query)
        {
            return query.ApplyRepositoryConstraints();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="addIncludes"></param>
        /// <returns></returns>
        protected IQueryable<TEntity> GetByIncludes(IQueryable<TEntity> query, Func<IQueryable<TEntity>, IQueryable<TEntity>> addIncludes)
        {
            var original = query;
            query = addIncludes?.Invoke(query);
            var returnQuery = query ?? original;
            return _asNoTracking ? returnQuery.AsNoTracking() : returnQuery;
        }
    }
}
