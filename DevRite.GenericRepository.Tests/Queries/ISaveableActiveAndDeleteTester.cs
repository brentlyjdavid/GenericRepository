using System;
using System.Linq;
using DevRite.GenericRepository.Tests.TestContext;
using DevRite.GenericRepository.Tests.TestContext.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DevRite.GenericRepository.Tests.Queries
{
    [TestClass]
    public class ISaveableActiveAndDeleteTester
    {
        [TestMethod]
        public void Queries_ActiveAndDelete_ReturnsCorrectRecords() {
            var repo = SetupDatabase();
            var records = repo.ActiveAndDeletes.ToList();
            Assert.IsTrue(records.Count == 2, "records.Count==2");
        }

        [TestMethod]
        public void Queries_ActiveAndDelete_ReturnsCorrectRecords2() {
            var repo = SetupDatabase();
            var records = Enumerable.ToList<ActiveAndDelete>(repo.ActiveAndDeletes.Where(m => m.IsActive));
            Assert.IsTrue(records.Count == 1, "records.Count==1");
        }

        private GenericRepositoryRepo SetupDatabase() {
            var repo = new GenericRepositoryRepo(new GenericRepositoryContext(Guid.NewGuid().ToString()),null, true);
            repo.ActiveAndDeletes.AddOrUpdateAndSave(new ActiveAndDelete() {
                Id = Guid.NewGuid().ToString(),
                IsActive = true,
                DateDeletedUtc = DateTime.UtcNow,
            });

            repo.ActiveAndDeletes.AddOrUpdateAndSave(new ActiveAndDelete() {
                Id = Guid.NewGuid().ToString(),
                IsActive = false,
                DateDeletedUtc = DateTime.UtcNow,
            });

            repo.ActiveAndDeletes.AddOrUpdateAndSave(new ActiveAndDelete() {
                Id = Guid.NewGuid().ToString(),
                IsActive = false,
            });

            repo.ActiveAndDeletes.AddOrUpdateAndSave(new ActiveAndDelete() {
                Id = Guid.NewGuid().ToString(),
                IsActive = true,
            });
            return repo;
        }
    }
}
