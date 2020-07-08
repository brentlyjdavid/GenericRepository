using System;

namespace DevRite.GenericRepository.Core.Interfaces {
    /// <summary>
    /// Start time of an item
    /// </summary>
    public interface ISaveableStart {
        /// <summary>
        /// Always UTC
        /// </summary>
        DateTime StartUtc { get; set; }
    }
}
