using System;
using System.Linq;
using DevRite.GenericRepository.Core.Interfaces;

#if NETSTANDARD
using Microsoft.EntityFrameworkCore;
#else
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
#endif

namespace DevRite.GenericRepository.Contexts
{
    /// <summary>
    /// Base Context for regular apps
    /// </summary>
    public class ApplicationBaseDbContext : DbContext
    {
        public bool AddViewsToContext { get; set; } 
#if NETSTANDARD
        /// <summary>
        /// Options 
        /// </summary>
        /// <param name="options"></param>
        public ApplicationBaseDbContext(DbContextOptions options) : base(options)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        private void RemoveCascadeDeletes(ModelBuilder builder)
        {
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(m => m.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //removes cascade delete database wide
            RemoveCascadeDeletes(builder);

            if (!AddViewsToContext) return;
            //add IEntityViews
            var interfaceType = typeof(IEntityView);
            var iEntityViews = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(m => m.GetTypes())
                .Where(p => interfaceType.IsAssignableFrom(p) && p.Name != interfaceType.Name)
                .ToList();

            foreach (var item in iEntityViews)
            {
                var iEntityItem = Activator.CreateInstance(item) as IEntityView;
                builder.Query(item)
                    .ToView(iEntityItem.GetViewName(), iEntityItem.GetSchema());
            }

        }
#else
        /// <summary>
        /// 
        /// </summary>
        public ApplicationBaseDbContext() : this("DefaultConnection")
        {
            AddViewsToContext = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameOrConnectionString"></param>
        public ApplicationBaseDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(DbModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            builder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public static TDbContext Create<TDbContext>()
        {
            return Activator.CreateInstance<TDbContext>();
        }
#endif

    }
}
