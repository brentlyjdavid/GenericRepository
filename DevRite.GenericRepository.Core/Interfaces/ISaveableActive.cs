namespace DevRite.GenericRepository.Core.Interfaces {
    /// <summary>
    /// Designates an active field on a table
    /// </summary>
    public interface ISaveableActive {
        /// <summary>
        /// The Active Flag
        /// </summary>
        bool IsActive { get; set; }
    }
}
