using Detached.Angular2Demo.Model;
using Detached.Angular2Demo.Server.Security.Invoices;
using Detached.Angular2Demo.Server.Security.Roles.Services;
using Detached.Angular2Demo.Server.Security.Users.Services;
using Detached.EntityFramework;
using Detached.EntityFramework.Plugins.ManyToMany;
using Detached.EntityFramework.Plugins.Seeding;
using Detached.Mvc.Errors;
using Detached.Mvc.Localization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Detached.Angular2Demo.Server
{
    public class Startup
    {
        IHostingEnvironment _environment;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile(@"Server\Settings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($@"Server\Settings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
            }
            Configuration = builder.Build();

            _environment = env;
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddSingleton<ITypeMetadataProvider, TypeMetadataProvider>();
            services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();

            // add Mvc.
            services.AddMvc(options =>
            {
                //options.Filters.Add(new GlobalExceptionFilter());
                //options.ModelMetadataDetailsProviders.Add(keyProvider);
            }).AddDataAnnotationsLocalization();

            // add Entity Framework.
            services.AddDbContext<DefaultContext>(ctx =>
                ctx.UseSqlServer(Configuration.GetConnectionString("Default"))
                   .UseDetached(dconf => dconf.UseManyToManyHelper()));

            services.AddScoped(typeof(IDetachedContext<>), typeof(DetachedContext<>));

            // add app services.
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IInvoiceService, InvoiceService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IDetachedContext<DefaultContext> detached)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true,
                    ConfigFile = "webpack.config.js"
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseMiddleware(typeof(ErrorHandlerMiddleware));

            var supportedCultures = new[]
                    {
                        new CultureInfo("en-US"),
                        new CultureInfo("en"),
                        new CultureInfo("es-AR"),
                        new CultureInfo("es"),
                    };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(new CultureInfo("es-AR")),
                FallBackToParentCultures = true,
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });

            detached.DbContext.Database.EnsureCreated();
            detached.SeedFromJsonFileAsync("./Server/Seed.json").GetAwaiter().GetResult();
        }
    }
}
