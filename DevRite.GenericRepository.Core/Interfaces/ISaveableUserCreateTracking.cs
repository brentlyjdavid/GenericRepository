using System;

namespace DevRite.GenericRepository.Core.Interfaces
{
    /// <summary>
    /// Tracks created by
    /// </summary>
    /// <typeparam name="TUserKey"></typeparam>
    public interface ISaveableUserCreateTracking<TUserKey> where TUserKey :IEquatable<TUserKey>
    {
        /// <summary>
        /// UserId who created it
        /// </summary>
        TUserKey CreatedById { get; set; }
    }
}
