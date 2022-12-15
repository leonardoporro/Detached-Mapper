using Detached.Mappers.EntityFramework;
using Detached.Mappers.Samples.RestApi.Models;
using Detached.Mappers.Samples.RestApi.Services;
using Detached.Mappers.Samples.RestApi.Stores;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using Detached.Mappers.Annotations;

namespace Detached.Mappers.Samples.RestApi
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

            services.AddDbContext<MainDbContext>(cfg =>
            {
                cfg.UseSqlServer(Configuration.GetConnectionString("MainDb"));
                cfg.UseDetached();
            });

            services.AddScoped<InvoiceService>();
            services.AddScoped<InvoiceStore>();

            services.Configure<MapperOptions>(m =>
            {
                m.Type<User>().Entity(true);
            });
        }

        public class User { }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, MainDbContext mainDb)
        {
            mainDb.Database.EnsureDeleted();
            mainDb.Database.EnsureCreated();
            Seed(mainDb);

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
                Name = "sample customer",
                Email = "samplecustomer@example.com",
                Phone = "123 456 7890"
            });
            db.StockUnits.Add(new StockUnit { Name = "Potato", Quantity = 10 });
            db.StockUnits.Add(new StockUnit { Name = "Tomato", Quantity = 8 });
            db.SaveChanges();

            db.Invoices.Add(new Invoice
            {
                Customer = db.Customers.Find(1),
                DateTime = DateTime.Now,
                Type = db.InvoiceTypes.Find(1),
                Rows = new List<InvoiceRow>
                {
                    new InvoiceRow
                    {
                        Description = "invoice row",
                        Quantity = 1,
                        SKU = db.StockUnits.Find(1),
                        UnitPrice = 100
                    }
                }
            });
            db.SaveChanges();
        }
    }
}