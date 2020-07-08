using System;

namespace DevRite.GenericRepository.Core.Interfaces {
    /// <summary>
    /// End time of an item
    /// </summary>
    public interface ISaveableEnd {
        /// <summary>
        /// Always UTC
        /// </summary>
        DateTime EndUtc { get; set; }
    }
}
