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
    public static class LoggerFactory 
    {
        public const string LoggerOutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";

        public const string AppInsightsKeySetting = "ApplicationInsights:InstrumentationKey";

        public const string LogFileName = "log.txt";

        public static Logger CreateLogger() 
        {
            var loggerConfig = new LoggerConfiguration()
                .WriteTo.File(
                    outputTemplate: LoggerOutputTemplate,
                    path: Path.Combine(Configuration.RootFolder, LogFileName),
                    rollingInterval: RollingInterval.Day)
                .ReadFrom.Configuration(Configuration.Instance);

            var appInsightsKey = Configuration.Instance.GetValue<string>(AppInsightsKeySetting, null);
            if (!string.IsNullOrEmpty(appInsightsKey))
            {
                // Note that AppInsights sink ignores the output template if given in the json configuration,
                // and we can't provide it in code as well.
                // See https://stackoverflow.com/questions/57183780/using-outputtemplate-argument-in-serilog-with-azure-application-insights
                loggerConfig = loggerConfig.WriteTo.ApplicationInsights(new TelemetryConfiguration()
                {
                    InstrumentationKey = appInsightsKey,
                    TelemetryChannel = new ServerTelemetryChannel(),
                }, TelemetryConverter.Traces);
            }

            var logger = loggerConfig.CreateLogger();

            if(string.IsNullOrEmpty(appInsightsKey)) 
            {
                logger.Warning($"{AppInsightsKeySetting} setting is not found or empty. ApplicationInsights logging is not enabled.");
            }

            return logger;
        }
    }
}