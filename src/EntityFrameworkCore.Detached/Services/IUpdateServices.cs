using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EntityFrameworkCore.Detached.Services
{
    public interface IUpdateServices
    {
        EntityEntry Add(object detached);

        EntityEntry Attach(object detached);

        bool Copy(object srcEntity, EntityEntry destEntry);

        void Delete(object persiste);
            
        EntityEntry Merge(object detachedObj, object persistedObj);
    }
}