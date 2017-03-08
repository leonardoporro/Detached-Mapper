using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Detached.EntityFramework.Services
{
    /// <summary>
    /// Provides persistence servies for detached contexts.
    /// </summary>
    public interface IUpdateServices
    {
        /// <summary>
        /// Recursively attaches the given entity and sets its state to Added.
        /// </summary>
        /// <param name="detached">The detached entity to add.</param>
        /// <returns>The EntityEntry for the given entity.</returns>
        EntityEntry Add(object detached);

        /// <summary>
        /// Attaches the given entity and sets its state to Attach.
        /// </summary>
        /// <param name="detached">The detached entity to attach.</param>
        /// <returns>The EntityEntry for the given entity.</returns>
        EntityEntry Attach(object detached);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="detachedObj"></param>
        /// <param name="persistedObj"></param>
        /// <returns></returns>
        EntityEntry Merge(object detachedObj, object persistedObj);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="srcEntity"></param>
        /// <param name="destEntry"></param>
        /// <returns></returns>
        bool Copy(object srcEntity, EntityEntry destEntry);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="persiste"></param>
        void Delete(object persisted);
    }
}