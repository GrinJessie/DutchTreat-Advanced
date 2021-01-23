using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DutchTreat
{
  public class Program
  {
    public static void Main(string[] args)
    {
      BuildWebHost(args).Run();
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
