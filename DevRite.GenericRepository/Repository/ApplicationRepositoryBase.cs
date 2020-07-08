using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using DevRite.GenericRepository.Interfaces;

#if !NETSTANDARD
using System.Data.Entity;
#else
using Microsoft.EntityFrameworkCore;
#endif

namespace DevRite.GenericRepository.Repository
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    public abstract class ApplicationRepositoryBase<TDbContext> : IApplicationRepository<TDbContext>
        where TDbContext : DbContext
    {
        /// <summary>
        /// Main entry to database
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="currentUser"></param>
        /// <param name="isHangfire"></param>
        protected ApplicationRepositoryBase(TDbContext ctx, IPrincipal currentUser = null, bool isHangfire = false)
        {
            Context = ctx;
            User = (currentUser as ClaimsPrincipal);
            IsHangfire = isHangfire;
            
            Initialize();
        }

        /// <summary>
        /// 
        /// </summary>
        public abstract void Initialize();


        /// <summary>
        /// This repositories context
        /// </summary>
        public TDbContext Context { get; set; }
        /// <summary>
        /// Current user
        /// </summary>
        public ClaimsPrincipal User { get; set; }

        /// <summary>
        /// Lets repo know it's running within hangfire
        /// </summary>
        public bool IsHangfire { get; private set; }


        /// <summary>
        /// Saves any items that are marked as 'changed' in the context
        /// </summary>
        /// <returns><code>true</code> if something got saved or <code>false</code> if nothing saved.  <code>false</code> does not indicate a failed save, but indicates that EF doesn't think anything needed to be changed</returns>
        public bool Save()
        {
            return Context.SaveChanges() > 0;
        }

        /// <summary>
        /// Saves any items that are marked as 'changed' in the context
        /// </summary>
        /// <returns><code>true</code> if something got saved or <code>false</code> if nothing saved.  <code>false</code> does not indicate a failed save, but indicates that EF doesn't think anything needed to be changed</returns>
        public async Task<bool> SaveAsync()
        {
            return (await Context.SaveChangesAsync()) > 0;
        }
    }
}
