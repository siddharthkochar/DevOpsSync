using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace DevOpsSync.WebApp.API
{
    public static class ProgramContext
    {
        public static IConfiguration Configuration =
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ENVIRONMENT") ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

        public static Action<IConfigurationBuilder> ConfigureHost = configurationBuilder =>
        {
            configurationBuilder.AddConfiguration(Configuration);
        };
    }
}
