using Detached.Angular2Demo.Model;
using Detached.Angular2Demo.Server.Security.Roles.Services;
using Detached.Angular2Demo.Server.Security.Users.Services;
using Detached.EntityFramework;
using Detached.EntityFramework.Plugins.ManyToMany;
using Detached.EntityFramework.Plugins.Seeding;
using Detached.Mvc.Localization;
using Detached.Mvc.Localization.Mapping;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Detached.Angular2Demo.Server
{
    public class Startup
    {
        public IHostingEnvironment Environment { get; }

        public IConfigurationRoot Configuration { get; }

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
            Environment = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // add a DbContext and configure it to use Detached, and some plugins.
            services.AddDbContext<DefaultContext>(ctx =>
                ctx.UseSqlServer(Configuration.GetConnectionString("Default"))
                   .UseDetached(dconf => dconf.UseManyToManyHelper()));

            // add a generic IDetachedContext to be injected later to services/controllers.
            services.AddScoped(typeof(IDetachedContext<>), typeof(DetachedContext<>));

            // add .resx localization.
            services.AddLocalization();

            // add MVC.
            services.AddMvc();

            // add a resource mapper.
            // this maps .NET type names to a key and resource source (file/table/etc).
            services.AddResourceMapper(o =>
            {
                o.SupportInfo = new SupportInfo { Email = "it@example.com" };
                o.StringCase = StringCase.PascalCase;
                o.FallbackKey = (feat, type, prop) => new ResourceKey { KeyName = $"{feat}_{type}_{prop}", ResourceName = "Server.Common.Resources.Resources" };
                o.Rules.Clear();
                o.Rules.Add(
                    new Rule(pattern: "{company}.{app}.{platform}.{module}.{feature}.{layer}.{modelOrController}.{fieldOrAction}#{descriptor}",
                             keyTemplate: "{modelOrController}_{fieldOrAction}_{descriptor}", 
                             sourceTemplate: "{platform}.{module}.{feature}.Resources.Resources"));
            })
            .AddAutomaticDisplayMetadataLocalization()
            .AddAutomaticValidationAttributeLocalization();

            // register app services.
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
        }

        public void Configure(IApplicationBuilder app,
                              IHostingEnvironment env,
                              ILoggerFactory loggerFactory,
                              IDetachedContext<DefaultContext> detached)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            // ensure db exists and add data from Seed.json
            if (detached.DbContext.Database.EnsureCreated())
            {
                detached.SeedFromJsonFileAsync("./Server/Seed.json").GetAwaiter().GetResult();
            }

            // add localization support
            CultureInfo[] supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("es-AR")
            };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(supportedCultures[0], supportedCultures[0]),
                FallBackToParentCultures = false,
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            // configure WebPack hot reload and error page
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

            // use Angular2 client files (.js, .html)
            app.UseStaticFiles();

            // use MVC with SPA fallback to support Angular2 routing
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
