using EntityFrameworkCore.Detached.Conventions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace EntityFrameworkCore.Detached
{
    /// <summary>
    /// Provides custom configuration parameters for EntityFramework.Detached.
    /// </summary>
    public class DetachedOptionsExtension : IDbContextOptionsExtension
    {
        IServiceCollection _detachedServiceCollection = new ServiceCollection();

        /// <summary>
        /// Gets an extension for the internal service collection of the DbContext.
        /// Allows to insert custom services into the internal service provider.
        /// </summary>
        public IServiceCollection DetachedServices
        {
            get
            {
                return _detachedServiceCollection;  
            }
        }

        /// <summary>
        /// Defines the behavior of detached update when an entity with a specified key value
        /// does not exist in the database. Usually happens if the entity was deleted or it never existed.
        /// Entities with no key value defined are automatically added.
        /// </summary>
        public bool ThrowExceptionOnEntityNotFound { get; set; }

        public void ApplyServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddDetachedEntityFramework();
            foreach (var service in DetachedServices)
            {
                serviceCollection.Add(service);
            }
        }
    }
}