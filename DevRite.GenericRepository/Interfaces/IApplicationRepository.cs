
using System.Security.Claims;
using System.Threading.Tasks;

#if  NETSTANDARD
using Microsoft.EntityFrameworkCore;
#else
using System.Data.Entity;
#endif

namespace DevRite.GenericRepository.Interfaces
{
    /// <summary>
    /// Application Repository interface
    /// </summary>
    public interface IApplicationRepository<TDbContext> where TDbContext : DbContext
    {
        /// <summary>
        /// The context for this database
        /// </summary>
        TDbContext Context { get; set; }
        /// <summary>
        /// 
        /// </summary>
        ClaimsPrincipal User { get; set; }

        /// <summary>
        /// Saves all pending entities
        /// </summary>
        /// <returns><code>true</code> if something got saved or <code>false</code> if nothing saved.  <code>false</code> does not indicate a failed save, but indicates that EF doesn't think anything needed to be changed</returns>
        bool Save();

        /// <summary>
        /// Saves all pending entities
        /// </summary>
        /// <returns><code>true</code> if something got saved or <code>false</code> if nothing saved.  <code>false</code> does not indicate a failed save, but indicates that EF doesn't think anything needed to be changed</returns>
        Task<bool> SaveAsync();




    }
}
