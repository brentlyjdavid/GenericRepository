using System;

namespace DevRite.GenericRepository.Core.Interfaces {
    /// <summary>
    /// DateCreated Field
    /// </summary>
    public interface ISaveableCreate {
        /// <summary>
        /// Always UTC
        /// </summary>
        DateTime DateCreatedUtc { get; set; }
    }
}
