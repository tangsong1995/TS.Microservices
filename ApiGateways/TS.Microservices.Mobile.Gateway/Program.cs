using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Com.Ctrip.Framework.Apollo;
using Com.Ctrip.Framework.Apollo.Logging;

namespace TS.Microservices.Mobile.Gateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostBuilderContext, configurationBuilder)=>
                {
                    LogManager.UseConsoleLogging(Com.Ctrip.Framework.Apollo.Logging.LogLevel.Trace);
                    configurationBuilder.AddApollo(configurationBuilder.Build().GetSection("Apollo"))
                        .AddDefault(Com.Ctrip.Framework.Apollo.Enums.ConfigFileFormat.Properties);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
            webBuilder.UseStartup<Startup>();
        });
    }
}
