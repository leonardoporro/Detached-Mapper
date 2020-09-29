using Detached.Mappers.EntityFramework.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace Detached.Mappers.EntityFramework
{
    public static class Package
    {
        public static void AddDetachedEntityFramework(this IServiceCollection services)
        {
            services.AddDetached();
            services.AddScoped<QueryProvider>();
        }
    }
}