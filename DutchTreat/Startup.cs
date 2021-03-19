using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using DutchTreat.Services;
using DutchTreatAdvanced.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace DutchTreat
{
    public class Startup
    {
        private readonly IConfiguration _config;

        // allow to inject certain interface that are set up in the Program.cs
        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // "server=(localdb)\\ProjectsV13;Database=DutchTreatDb;Integrated Security=true;MultipleActiveResultSets=true;" 
            //server name can be checked in the view -> SQL server object explorer
            // Integrated Security can be replaced with real credential on deployment
            // MultipleActiveResultSets is an EF core special connection - retrieve multiple steams of data at the same time
            // SCOPED service
            services.AddDbContext<Dutchcontext>(configuration =>
            {
                configuration.UseSqlServer(_config.GetConnectionString("DutchConnectionString"));
            }, ServiceLifetime.Transient);

                    services.AddTransient<IMailService, NullMailService>();
            // Support for real mail service

            // Will be creatable through the dependance injection
            services.AddTransient<DutchSeeder>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Add IDutchRepository as a service people can use, but use as the implementation this version (DutchRepository)
            // In testing, can use services.AddScoped<IDutchRepository, MockDutchRepository>();
            services.AddScoped<IDutchRepository, DutchRepository>();

            services.AddControllersWithViews()
                // Some new features after 2.1 on API controller
                .SetCompatibilityVersion(CompatibilityVersion.Latest)
                // Error is the default; Serialize creates hugely deep nested object graph;  
                .AddNewtonsoftJson(option => option.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseNodeModules();

            app.UseRouting();

            app.UseEndpoints(cfg =>
            {
                cfg.MapControllerRoute("Default",
                "{controller}/{action}/{id?}",
                new { controller = "App", Action = "Index" });
            });

        }
    }
}
