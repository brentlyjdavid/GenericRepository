using System;
using DevRite.GenericRepository.Tests.TestContext;
using DevRite.GenericRepository.Tests.TestContext.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DevRite.GenericRepository.Tests.Saving
{
    [TestClass]
    public class SavingTester
    {
        [TestMethod]
        public void Saving_DateCreated()
        {
            var repo = SetupDatabase();
            var item = repo.DateCreateds.FirstOrDefault();
            Assert.IsTrue(item.DateCreatedUtc != DateTime.MinValue);
        }

        [TestMethod]
        public void Saving_DateCreatedAlreadySet()
        {
            var repo = SetupDatabaseWithDateCreated(new DateTime(2018, 1,1));
            var item = repo.DateCreateds.FirstOrDefault();
            Assert.IsTrue(item.DateCreatedUtc == new DateTime(2018, 1, 1));
        }

        [TestMethod]
        public void Saving_DateModified()
        {
            var repo = SetupDatabase();
            var item = repo.DateCreateds.FirstOrDefault();
            Assert.IsTrue(item.DateLastModifiedUtc != DateTime.MinValue);
        }

        private GenericRepositoryRepo SetupDatabase()
        {
            var repo = new GenericRepositoryRepo(new GenericRepositoryContext(Guid.NewGuid().ToString()),null, true);
            repo.DateCreateds.AddOrUpdateAndSave(new DateCreated());
            return repo;
        }

        private GenericRepositoryRepo SetupDatabaseWithDateCreated(DateTime date)
        {
            var repo = new GenericRepositoryRepo(new GenericRepositoryContext(Guid.NewGuid().ToString()), null,true);
            repo.DateCreateds.AddOrUpdateAndSave(new DateCreated()
            {
                DateCreatedUtc = date,
            });
            return repo;
        }
    }
}
