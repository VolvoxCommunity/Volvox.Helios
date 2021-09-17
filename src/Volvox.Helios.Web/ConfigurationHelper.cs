using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Volvox.Helios.Web
{
    public static class ConfigurationHelper
    {
        public static IConfiguration GetDefaultConfiguration()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("./appsettings.json", false, true)
                .AddJsonFile($"./appsettings.{environment}.json", true, true)
                // .AddJsonFile("./modulemetadata.json")
                .AddUserSecrets<Startup>().Build();
        }
    }
}