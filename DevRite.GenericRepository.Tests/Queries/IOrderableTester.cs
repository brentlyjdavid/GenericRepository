using System;
using System.Linq;
using DevRite.GenericRepository.Tests.TestContext;
using DevRite.GenericRepository.Tests.TestContext.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DevRite.GenericRepository.Tests.Queries
{
    [TestClass]
    public class IOrderableTester
    {
        [TestMethod]
        public void IOrderable_ReturnsOrdered() {
            var repo = SetupDatabase();
            var records = repo.Orderables.ToList();

            Assert.IsTrue(Enumerable.First<Orderable>(records).Id == "444" && Enumerable.Last<Orderable>(records).Id =="111");
        }

        [TestMethod]
        public void IOrderableDescending_ReturnsOrderedDescending() {
            var repo = SetupDatabase();
            var records = repo.OrderableDescendings.ToList();

            Assert.IsTrue(Enumerable.First<OrderableDescending>(records).Id == "555" && Enumerable.Last<OrderableDescending>(records).Id =="888");
        }
        

        private GenericRepositoryRepo SetupDatabase() {
            var ctx = new GenericRepositoryRepo(new GenericRepositoryContext(Guid.NewGuid().ToString()),null,true);
            ctx.Orderables.AddOrUpdateAndSave(new Orderable() {
                DisplayOrder = 10,
                Id = "111",
            });

            ctx.Orderables.AddOrUpdateAndSave(new Orderable() {
                DisplayOrder = 8,
                Id = "222",
            });

            ctx.Orderables.AddOrUpdateAndSave(new Orderable() {
                DisplayOrder = 6,
                Id = "333",
            });

            ctx.Orderables.AddOrUpdateAndSave(new Orderable() {
                DisplayOrder = 4,
                Id = "444",
            });

            ctx.OrderableDescendings.AddOrUpdateAndSave(new OrderableDescending() {
                DisplayOrder = 4,
                Id = "888",
            });

            ctx.OrderableDescendings.AddOrUpdateAndSave(new OrderableDescending() {
                DisplayOrder = 6,
                Id = "777",
            });

            ctx.OrderableDescendings.AddOrUpdateAndSave(new OrderableDescending() {
                DisplayOrder = 8,
                Id = "666",
            });

            ctx.OrderableDescendings.AddOrUpdateAndSave(new OrderableDescending() {
                DisplayOrder = 10,
                Id = "555",
            });

            return ctx;
        }
    }
}
