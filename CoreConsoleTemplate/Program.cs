using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CoreConsoleTemplate
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Build()).CreateLogger();

            var host = Host.CreateDefaultBuilder().ConfigureServices((context, services) =>
            {
                services.AddScoped<IDataAccess, DataAccess>();
            }).UseSerilog().Build();

            var _main = ActivatorUtilities.CreateInstance<Main>(host.Services);
            _main.Run();
        }

        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).AddJsonFile($"apsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true, reloadOnChange: true).AddEnvironmentVariables();
        }
    }
}
