using System.Security.Principal;
using DevRite.GenericRepository.Interfaces;
using DevRite.GenericRepository.Repository;
using DevRite.GenericRepository.Tests.TestContext.Entities;

namespace DevRite.GenericRepository.Tests.TestContext
{
    public class GenericRepositoryRepo : ApplicationRepositoryBase<GenericRepositoryContext>
    {
        public IRepository<string, JustActive, string> JustActives { get; set; }
        public IRepository<string, ActiveAndDelete, string> ActiveAndDeletes { get; set; }
        public IRepository<string, DateCreated, string> DateCreateds { get; set; }
        public IRepository<string, Orderable, string> Orderables { get; set; }
        public IRepository<string, OrderableDescending, string> OrderableDescendings { get; set; }

        public GenericRepositoryRepo(GenericRepositoryContext ctx, IPrincipal currentUser, bool isHangfire) : base(ctx, currentUser, isHangfire)
        {
            
        }

        public override void Initialize()
        {
            JustActives = new Repository<string, JustActive, string>(Context, User, IsHangfire);
            ActiveAndDeletes = new Repository<string, ActiveAndDelete, string>(Context, User, IsHangfire);
            DateCreateds = new Repository<string, DateCreated, string>(Context, User, IsHangfire);
            Orderables = new Repository<string, Orderable, string>(Context, User, IsHangfire);
            OrderableDescendings = new Repository<string, OrderableDescending, string>(Context, User, IsHangfire);
        }
    }
}
