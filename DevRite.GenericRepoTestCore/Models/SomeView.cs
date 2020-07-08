using DevRite.GenericRepository.Core.Interfaces;

namespace DevRite.GenericRepoTestCore.Models
{
    public class SomeView : IEntityView
    {
        public string Id { get; set; }
        public string GetSchema()
        {
            return "dbo";
        }

        public string GetViewName()
        {
            return "SomeViewsView";
        }
    }
}
