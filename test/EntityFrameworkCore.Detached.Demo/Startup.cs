using EntityFrameworkCore.Detached.Contracts;
using EntityFrameworkCore.Detached.Demo.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Demo
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            services.AddEntityFrameworkDetached();
            services.AddSessionInfoProvider(() => "Current User");
            services.AddDbContext<MainDbContext>(cfg => cfg.UseDetached().UseSqlServer(Configuration.GetConnectionString("Default")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, MainDbContext dbContext)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();

            ConfigureContext(dbContext).Wait();
        }

        public async Task ConfigureContext(MainDbContext dbContext)
        {
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            dbContext.AddRange(new[]
            {
                new SellPointType { Name = "Type 1" },
                new SellPointType { Name = "Type 2" },
                new SellPointType { Name = "Type 3" },
                new SellPointType { Name = "Type 4" }
            });
            await dbContext.SaveChangesAsync();

            DetachedContext<MainDbContext> detached = new DetachedContext<MainDbContext>(dbContext, new DelegateSessionInfoProvider(() => "System"));
            await detached.UpdateAsync(new Company()
            {
                Name = "Hello Company!",
                SellPoints = new List<SellPoint>(new[]
                {
                    new SellPoint { Name = "USA", Address = "9999 12th Avenue Seattle, WA 98122, EE. UU.", Type = new SellPointType { Id = 1 }},
                    new SellPoint { Name = "Argentina", Address = "Mitre 564, Rosario, Santa Fe, Argentina", Type = new SellPointType { Id = 1 } },
                    new SellPoint { Name = "Germany", Address = "Gartenstraße 1000, 30161 Hannover, Germany", Type = new SellPointType { Id = 1 } },
                    new SellPoint { Name = "France", Address = "500 Rue Saint-Paul 34000 Montpellier, France", Type = new SellPointType { Id = 1 } }
                })
            });
            await detached.SaveChangesAsync();
        }
    }
}
