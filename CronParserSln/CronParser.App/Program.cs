using CronParser.App.Services;
using CronParser.Lib.Extensions;
using CronParser.Lib.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace CronParser.App
{
    class Program
    {
        static void Main(string[] args)
        {
            
            try
            {
                var cmdArgs = CronExpressionReader.Read(args);

                var host = CreateHost(args);
                using var serviceScope = host.Services.CreateScope();
                var services = serviceScope.ServiceProvider;
                var parser = services.GetRequiredService<ICronExpressionParser>();
                parser.Parse(cmdArgs);
                parser.WriteToOutput();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{DateTime.Now} : exception thrown during execution. error : {ex.GetBaseException().Message}");
            }
        }

        private static IHost CreateHost(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args);

            hostBuilder.ConfigureServices(ConfigureServices) ;
            hostBuilder.ConfigureLogging((context , bldr) =>
            {
                bldr.SetMinimumLevel(LogLevel.Information);
            });
            return hostBuilder.Build();
        }

        private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            var configuration = context.Configuration;
            services.ConfigureParserServices();
            services.AddSingleton<IOutputWriter, OutputWriter>();
        }
    }
}
