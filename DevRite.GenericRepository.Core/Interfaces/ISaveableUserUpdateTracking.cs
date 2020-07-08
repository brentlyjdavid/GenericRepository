using System;

namespace DevRite.GenericRepository.Core.Interfaces
{
    /// <summary>
    /// Tracks Updated By
    /// </summary>
    /// <typeparam name="TUserKey"></typeparam>
    public interface ISaveableUserUpdateTracking<TUserKey> where TUserKey : IEquatable<TUserKey>
    {
        /// <summary>
        /// Userid who last modified it
        /// </summary>
        TUserKey LastModifiedById { get; set; }
    }
}
