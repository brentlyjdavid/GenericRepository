namespace DevRite.GenericRepository.Core.Interfaces {
    /// <summary>
    /// Entity that is tracked by a save
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface ISaveable<TKey>{
        /// <summary>
        /// Id - Points to primary key of table
        /// </summary>
        TKey Id { get; set; }
    }
}
