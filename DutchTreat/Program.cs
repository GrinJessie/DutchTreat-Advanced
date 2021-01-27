using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DutchTreatAdvanced.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
 
namespace DutchTreat
{
  public class Program
  {
    public static void Main(string[] args)
    {
        // let seeding happen way before building the web server
        var host = BuildWebHost(args);

        // Keep in main() to make sure that every time the web server is started, it will attempt to seed the database to make sure all the part the database required is there including running migration
        // On start up only
        // No need for ef commands
        // Getting away from dropping DB and writing migrations
        SeedDb(host);

        BuildWebHost(args).Run();
    }

    private static void SeedDb(IWebHost host)
    {
        // A way outside of standard web server to create a scope
        // During every request, it creates scope for the lifetime of the request
        // Get an instance of the DutchContext object that is true through out the entire request
        // But outside of that, there isn't a default scope unless we created one 
        var scopeFactory = host.Services.GetService<IServiceScopeFactory>();

        // Use using so the scope is closed once the work is done
        using var scope = scopeFactory?.CreateScope();
        // get the service within the context of the scope
        var seeder = scope?.ServiceProvider.GetService<DutchSeeder>();
        seeder?.Seed();
    }

    public static IWebHost BuildWebHost(string[] args) =>
        // CreateDefaultBuilder also sets up a default configuration file that we can use
        WebHost.CreateDefaultBuilder(args)
            // order matters - right after CreateDefaultBuilder
            .ConfigureAppConfiguration(SetupConfiguration)
            .UseStartup<Startup>()
            .Build();

    private static void SetupConfiguration(WebHostBuilderContext ctx, IConfigurationBuilder builder)
    {
        // Remove the default configuration options
        builder.Sources.Clear();

        // ReloadOnChange - reload when every time a change made to it so no need to restart the app
        // can be problematic in ASP.Net as change in web.config can cause the web server to restart the app
        // Can chain the add file method if you have multiple configurations, all will be mixed to a single set of configuration
        // If there are conflicts, it use the order of adding files to override - the later the more trustworthy
        builder.AddJsonFile("config.json", false, true)
                // .AddXmlFile("config.xml", true)
                // works well for IT deployment or cloud deployment - set things in development/production time without having to manage the config file to actually store secret of them
                .AddEnvironmentVariables();
    }
  }
}
