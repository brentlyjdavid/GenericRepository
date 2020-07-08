using DevRite.GenericRepository.Core.Interfaces;

namespace DevRite.GenericRepoTestCore.Models
{
    public class TestClass : ISaveable<string>
    {
        public string Id { get; set; }

    }

    public class TestComposite
    {
        public string Id1 { get; set; }
        public string Id2 { get; set; }
    }
}
