using EntityFrameworkCore.Detached.Demo.Model;
using EntityFrameworkCore.Detached.Demo.Services;
using EntityFrameworkCore.Detached.Plugins.Auditing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


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
            string connectionString = Configuration.GetConnectionString("Default");

            // Add framework services.
            services.AddMvc();
            services.AddTransient(typeof(IDetachedContext<>), typeof(DetachedContext<>));

            services.AddDbContext<MainDbContext>(cfg =>
                cfg.UseSqlServer(connectionString)
                   .UseDetached(opts => opts.UseAuditing()
                                            .AddSingleton<ISessionInfoProvider>(SessionInfoProvider.Default)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, MainDbContext dbContext)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseDeveloperExceptionPage();

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}