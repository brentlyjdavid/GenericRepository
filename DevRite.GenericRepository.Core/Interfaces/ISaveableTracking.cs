using System;

namespace DevRite.GenericRepository.Core.Interfaces
{
    /// <summary>
    /// Tracks Last Modified Date
    /// </summary>
    public interface ISaveableTracking
    {
        /// <summary>
        /// Always UTC
        /// </summary>
        DateTime? DateLastModifiedUtc { get; set; }
    }
}
