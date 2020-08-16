using Detached.EntityFramework;
using Detached.Samples.RestApi.Models;
using Detached.Samples.RestApi.Services;
using Detached.Samples.RestApi.Stores;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Detached.Samples.RestApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen();


            services.AddDetachedEntityFramework();
            services.AddDbContext<MainDbContext>(cfg =>
            {
                cfg.UseSqlServer(Configuration.GetConnectionString("MainDb"));
            });

            services.AddScoped<InvoiceService>();
            services.AddScoped<InvoiceStore>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, MainDbContext mainDb)
        {
            if (mainDb.Database.EnsureCreated())
            {
                Seed(mainDb);
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        void Seed(MainDbContext db)
        {
            db.InvoiceTypes.Add(new InvoiceType { Name = "Taxed" });
            db.Customers.Add(new Customer
            {
                DocumentNumber = "123",
                FirstName = "sample",
                LastName = "customer",
                Email = "samplecustomer@example.com"
            });
            db.StockUnits.Add(new StockUnit { Name = "Potato", Quantity = 10 });
            db.StockUnits.Add(new StockUnit { Name = "Tomato", Quantity = 8 });
            db.SaveChanges();
        }
    }
}