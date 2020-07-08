using DevRite.GenericRepository.Tests.TestContext.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevRite.GenericRepository.Tests.TestContext {
    public class GenericRepositoryContext : DbContext {
        private string Name { get; set; }
        public DbSet<JustActive> JustActives { get; set; }
        public DbSet<ActiveAndDelete> ActiveAndDeletes { get; set; }
        public DbSet<DateCreated> DateCreateds { get; set; }
        public DbSet<Orderable> Orderables { get; set; }
        public DbSet<OrderableDescending> OrderableDescendings { get; set; }
        public GenericRepositoryContext() {
            
        }

        public GenericRepositoryContext(string name) {
            Name = name;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseInMemoryDatabase(Name);
        }
    }
}
