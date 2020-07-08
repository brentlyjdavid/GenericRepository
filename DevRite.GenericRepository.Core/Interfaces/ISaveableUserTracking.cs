using System;

namespace DevRite.GenericRepository.Core.Interfaces
{
    /// <summary>
    /// Tracks created by and last modified by on a user
    /// </summary>
    public interface ISaveableUserTracking<TUserKey> : ISaveableUserUpdateTracking<TUserKey>, ISaveableUserCreateTracking<TUserKey> where TUserKey: IEquatable<TUserKey>
    {
    }
}
