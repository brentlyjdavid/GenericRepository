using DevRite.GenericRepository.Core.Interfaces;

namespace DevRite.GenericRepoTestCore.Views
{
    public class TestClassView : ISaveable<string>
    {
        public string Id { get; set; }
    }
}
