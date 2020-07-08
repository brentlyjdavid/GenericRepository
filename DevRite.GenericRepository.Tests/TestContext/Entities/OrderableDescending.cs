using DevRite.GenericRepository.Core.Interfaces;

namespace DevRite.GenericRepository.Tests.TestContext.Entities
{
    public class OrderableDescending :ISaveable<string>, IOrderableDescending
    {
        public string Id { get; set; }
        public int DisplayOrder { get; set; }
    }
}
