using System;
using System.Linq;
using DevRite.GenericRepository.Tests.TestContext;
using DevRite.GenericRepository.Tests.TestContext.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DevRite.GenericRepository.Tests.Queries
{
    [TestClass]
    public class ISaveableActiveTester
    {
        [TestMethod]
        public void ISaveableActive_ReturnsCorrectRecords() {
            var repo = SetupDatabase();
            var records = repo.JustActives.ToList();
            Assert.IsTrue(records.Count == 2, "records.Count==2");
        }

        [TestMethod]
        public void ISaveableActive_ReturnsCorrectRecordsBadQuery() {
            var repo = SetupDatabase();
            var records = Enumerable.ToList<JustActive>(repo.JustActives.Where(m => m.IsActive == false));
            Assert.IsTrue(records.Count ==0, "records.Count==0");
        }
        

        private GenericRepositoryRepo SetupDatabase() {
            var ctx = new GenericRepositoryRepo(new GenericRepositoryContext(Guid.NewGuid().ToString()),null,true);
            ctx.JustActives.AddOrUpdateAndSave(new JustActive() {
                IsActive = true,
            });

            ctx.JustActives.AddOrUpdateAndSave(new JustActive() {
                IsActive = false,
            });

            ctx.JustActives.AddOrUpdateAndSave(new JustActive() {
                IsActive = true,
            });

            ctx.JustActives.AddOrUpdateAndSave(new JustActive() {
                IsActive = false,
            });

            return ctx;
        }
    }
}
