using EntityFrameworkCore.Detached.Conventions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace EntityFrameworkCore.Detached
{
    public class DetachedOptionsExtension : IDbContextOptionsExtension
    {
        IServiceCollection _detachedServiceCollection = new ServiceCollection();

        public IServiceCollection DetachedServices
        {
            get
            {
                return _detachedServiceCollection;
            }
        }

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