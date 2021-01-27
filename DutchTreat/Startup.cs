using System;
using System.Collections.Generic;
using System.Linq;
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
            services.AddTransient<IMailService, NullMailService>();
            // Support for real mail service

            services.AddControllersWithViews();

            // "server=(localdb)\\ProjectsV13;Database=DutchTreatDb;Integrated Security=true;MultipleActiveResultSets=true;" 
            //server name can be checked in the view -> SQL server object explorer
            // Integrated Security can be replaced with real credential on deployment
            // MultipleActiveResultSets is an EF core special connection - retrieve multiple steams of data at the same time
            // SCOPED service
            services.AddDbContext<Dutchcontext>(configuration => configuration.UseSqlServer(_config.GetConnectionString("DutchConnectionString")));

            // Will be creatable through the dependance injection
            services.AddTransient<DutchSeeder>();

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
