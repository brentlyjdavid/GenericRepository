using System;

namespace DevRite.GenericRepository.Core.Interfaces {
    /// <summary>
    /// Marks as a deleted record
    /// </summary>
    public interface ISaveableDelete {
        /// <summary>
        /// Nullable Date always in UTC
        /// </summary>
        DateTime? DateDeletedUtc { get; set; }
    }
}
