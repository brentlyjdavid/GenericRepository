using DevRite.GenericRepository.Core.Interfaces;

namespace DevRite.GenericRepository.Tests.TestContext.Entities
{
    public class Orderable:ISaveable<string>, IOrderable
    {
        public string Id { get; set; }
        public int DisplayOrder { get; set; }
    }
}
