namespace DevRite.GenericRepository.Core.Interfaces
{
    /// <summary>
    /// This interface is used to determine which types are required to add the db context
    /// </summary>
    public interface IEntityView
    {
        string GetSchema();
        string GetViewName();
    }
}
