using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AspNetCore.Web.Api
{
    /// <summary>
    /// The main program class.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main entry point for the web api.
        /// </summary>
        /// <param name="args">An array of arguments when starting the web api.</param>
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// The web builder create method.
        /// </summary>
        /// <param name="args">An array of arguments when starting the web api.</param>
        /// <returns>Returns the web builder.</returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.ClearProviders();
                    logging.AddDebug();
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                })
                .UseStartup<Startup>();
    }
}
