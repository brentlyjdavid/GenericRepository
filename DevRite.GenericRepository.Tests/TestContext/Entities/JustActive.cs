using DevRite.GenericRepository.Core.Interfaces;

namespace DevRite.GenericRepository.Tests.TestContext.Entities {
    public class JustActive : ISaveable<string>, ISaveableActive {
        public string Id { get; set; }
        public bool IsActive { get; set; }
    }
}
