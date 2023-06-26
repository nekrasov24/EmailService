using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Filters;

namespace EMailService
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var ls = new LoggingLevelSwitch();
            var isService = Matching.FromSource("LogService.Subscribe");
            ls.MinimumLevel = ((LogEventLevel)1 + (int)LogEventLevel.Fatal);

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Filter.ByIncludingOnly(isService)
                .WriteTo
                .File(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs/log.txt"), fileSizeLimitBytes: 1_000_000,
                rollOnFileSizeLimit: true,
                shared: true,
                flushToDiskInterval: TimeSpan.FromSeconds(1))
                .CreateLogger();
            try
            {
                Log.Information("Starting web host");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
