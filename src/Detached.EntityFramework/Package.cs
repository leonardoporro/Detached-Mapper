using Detached.EntityFramework.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace Detached.EntityFramework
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