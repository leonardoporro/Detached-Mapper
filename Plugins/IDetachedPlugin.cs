using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Plugins
{
    public interface IDetachedPlugin
    {
        int Priority { get; }

        IInternalDetachedContext DetachedContext { get; set; }

        DbContext DbContext { get; }

        string Name { get; }

        bool IsEnabled { get; set; }

        Task OnEntityAdding(object detachedEntity, INavigation navigation, EntityEntry parentEntityEntry);

        Task OnEntityAdded(EntityEntry newEntityEntry, INavigation navigation, EntityEntry parentEntityEntry);

        Task OnEntityMerging(object detachedEntity, object persistedEntity, INavigation navigation, EntityEntry parentEntityEntry);

        Task OnEntityMerged(object detachedEntity, EntityEntry persistedEntityEntry,  INavigation navigation, bool modified, EntityEntry parentEntityEntry);

        Task OnEntityDeleting(object persistedEntity, INavigation navigation, EntityEntry parentEntityEntry);

        Task OnEntityDeleted(EntityEntry persistedEntityEntry, INavigation navigation, EntityEntry parentEntityEntry);

        Task OnEntityAttaching(object newEntity, INavigation navigation, EntityEntry parentEntityEntry);

        Task OnEntityAttached(EntityEntry newEntityEntry, INavigation navigation, EntityEntry parentEntityEntry);

        Task OnPropertyChanging(object oldValue, object newValue, IProperty property);

        Task OnPropertyChanged(object oldValue, object newValue, IProperty property);

        Task OnEntityLoading<TEntity>(IQueryable<TEntity> query);

        Task OnEntityLoaded(object entity);
    }
}