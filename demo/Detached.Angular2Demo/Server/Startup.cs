using Detached.Angular2Demo.Model;
using Detached.Angular2Demo.Server.Security.Invoices;
using Detached.Angular2Demo.Server.Security.Roles.Services;
using Detached.Angular2Demo.Server.Security.Users.Services;
using Detached.EntityFramework;
using Detached.EntityFramework.Plugins.ManyToMany;
using Detached.EntityFramework.Plugins.Seeding;
using Detached.Mvc;
using Detached.Mvc.Localization;
using Detached.Mvc.Metadata;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

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
            #region Database

            // add a DbContext and configure it to use Detached, and some plugins.
            services.AddDbContext<DefaultContext>(ctx =>
                ctx.UseSqlServer(Configuration.GetConnectionString("Default"))
                   .UseDetached(dconf => dconf.UseManyToManyHelper()));

            // add a generic IDetachedContext to be injected later to services/controllers.
            // IDetachedContext generic parameter (DbContext) is resolved recursively.
            services.AddScoped(typeof(IDetachedContext<>), typeof(DetachedContext<>));

            #endregion

            #region Mvc

            services.AddMvc(options =>
            {
            }).AddDataAnnotationsLocalization();

            #endregion

            #region Localization

            services.AddJsonLocalization();
            services.AddLocalizationMetadata();

            #endregion

            // register app services.
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IInvoiceService, InvoiceService>(); 
        }

        public void Configure(IApplicationBuilder app, 
                              IHostingEnvironment env, 
                              ILoggerFactory loggerFactory, 
                              IDetachedContext<DefaultContext> detached,
                              IMetadataProvider metadataProvider,
                              IJsonStringLocalizerFactory localizerFactory)
        {
            #region Logging

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            #endregion

            #region Database

            // ensure db exists and add data from Seed.json
            detached.DbContext.Database.EnsureCreated();
            detached.SeedFromJsonFileAsync("./Server/Seed.json").GetAwaiter().GetResult();

            #endregion

            #region Localization

            // this provides generated keys for types and properties, based on their Clr namespace and names.
            metadataProvider.Patterns.Clear();
            
            // this generates keys like core.validation.required.errorMessage, for .NET built-in validators
            metadataProvider.Patterns.Add(new Pattern(@"\bSystem.ComponentModel.DataAnnotations.\b(?<class>[\w]+)Attribute(?:\+(?<property>[\w]+))?$",
                                          new Dictionary<string, string> { { "module", "core" }, { "feature", "validation" } }));

            // this generates keys like users.user.name.displayName for app classes.
            metadataProvider.Patterns.Add(new Pattern(@"(?<module>[\w]+)\.(?<feature>[\w]+)\.(?:[\w]+)\.(?<class>[\w]+)(?:\+(?<property>[\w]+))?(?:\#(?<metaproperty>[\w]+))?$"));

            // this loads a directory with json files, and provides the available cultures and modules.
            // file content is loaded when needed.
            localizerFactory.Configure(@".\wwwroot\lang", new CultureInfo("en"));

            // configure localization based on the factory.
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(localizerFactory.DefaultCulture, localizerFactory.DefaultCulture),
                FallBackToParentCultures = false,
                SupportedCultures = localizerFactory.Cultures.ToList(),
                SupportedUICultures = localizerFactory.Cultures.ToList()
            });

            #endregion

            #region Mvc

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

            #endregion
        }
    }
}
