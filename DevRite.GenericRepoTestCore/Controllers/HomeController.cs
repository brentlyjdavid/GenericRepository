using System.Diagnostics;
using System.Linq;
using DevRite.GenericRepoTestCore.Models;
using DevRite.GenericRepoTestCore.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DevRite.GenericRepoTestCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppRepository _repo;
        public HomeController(AppRepository repo)
        {
            _repo = repo;
        }
        public IActionResult Index()
        {
            var items = _repo.TestClasses.ToList();
            var items2 = _repo.TestComposites.ToList();
            var items3 = _repo.TestClassesView.ToList();

            var test = Enumerable.ToList<SomeView>(_repo.Context.Query<SomeView>());
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
