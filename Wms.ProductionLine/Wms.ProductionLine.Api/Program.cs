using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace Wms.ProductionLine.Api
{
    public class Program
    {        
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            try
            {
                logger.Debug("init main");
                var host = BuildWebHost(args);

                var isService = !(Debugger.IsAttached || args.Contains("--console"));

                if (isService)
                {
                    host.RunAsCustomService();
                }
                else
                {
                    host.Run();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", true)
                .AddJsonFile("ef-configuration.json")
                .AddJsonFile("rawrabbit.json")
                .AddJsonFile("/app/secrets/rawrabbit.json", optional: true)
                .AddJsonFile("/app/secrets/ef-configuration.json", optional: true)
                .AddCommandLine(args)
                .Build();

            var webHostArgs = args.Where(arg => arg != "--console").ToArray();

            return WebHost.CreateDefaultBuilder(webHostArgs)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .UseNLog()
                .UseConfiguration(config)
                .UseHttpSys(options => { options.UrlPrefixes.Add("http://*:8001/api/production-line"); })
                .UseStartup<Startup>()
                .Build();
        }
    }
}
