using DevRite.GenericRepository.Interfaces;
using DevRite.GenericRepository.Repository;
using DevRite.GenericRepository.View;
using DevRite.GenericRepoTestCore.Context;
using DevRite.GenericRepoTestCore.Models;
using DevRite.GenericRepoTestCore.Views;
using Microsoft.AspNetCore.Http;

namespace DevRite.GenericRepoTestCore.Repositories
{
    public class AppRepository : ApplicationRepositoryBase<AppDbContext>
    {
        public IRepository<string, TestClass, string> TestClasses { get; set; }
        public IRepositoryCompositKeys<TestComposite, string> TestComposites { get; set; }

        public IView<TestClassView> TestClassesView { get; set; }
        public AppRepository(AppDbContext ctx, IHttpContextAccessor ctxAccessor, bool isHangfire = false) : base(ctx, ctxAccessor.HttpContext?.User, isHangfire)
        {
        }

        public override void Initialize()
        {
            TestClasses = new Repository<string, TestClass, string>(Context, User, IsHangfire);
            TestComposites = new Repository<TestComposite, string>(Context, User, IsHangfire);
            TestClassesView = new View<TestClassView>(Context, User, IsHangfire);
        }
    }
}
