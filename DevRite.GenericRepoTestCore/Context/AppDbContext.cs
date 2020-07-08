using DevRite.GenericRepository.Contexts;
using DevRite.GenericRepoTestCore.Models;
using Microsoft.EntityFrameworkCore;

namespace DevRite.GenericRepoTestCore.Context
{
    public class AppDbContext : ApplicationBaseDbContext
    {
        public DbSet<TestClass> TestClasses { get; set; }
        public DbSet<TestComposite> TestComposites { get; set; }
        
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            AddViewsToContext = true;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TestComposite>(options =>
            {
                options.HasKey(k=> new{k.Id1,k.Id2});
            });
        }
    }
}
