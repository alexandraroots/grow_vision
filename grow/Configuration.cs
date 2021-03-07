using Serilog.Core;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;
using System;
using Serilog;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;

namespace growtimelapse 
{
    public static class Configuration
    {
        private static string rootFolder;
        private static IConfigurationRoot configuration;

        public const string ConfigFileName = "config.json";

        public static string RootFolder
        {
            get 
            {
                if(rootFolder == null)
                {
                    rootFolder = Path.GetDirectoryName(Assembly.GetEntryAssembly().ManifestModule.FullyQualifiedName);
                }
                return rootFolder;
            }
        }

        public static IConfigurationRoot Instance
        {
            get 
            {
                if(configuration == null) 
                {
                    configuration = new ConfigurationBuilder()
                        .AddJsonFile(Path.Combine(rootFolder, ConfigFileName), true)
                        .Build();
                }
                return configuration;
            }
        }
    }
}