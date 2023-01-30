using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;

namespace API.RulesEngine.Logging
{
    public static class Logger
    {
        internal static void CreateSerilogConfiguration()
        {
            var env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

            var appsettings = env == "Development" ? $"appsettings.{env}.json" : "appsettings.json";

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(appsettings, optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var logEventLevel = config["AppSettings:LogLevel"];

            var logLevelDictionary = new Dictionary<string, LogEventLevel>
            {
                ["Debug"] = LogEventLevel.Debug,
                ["Information"] = LogEventLevel.Information,
                ["Warning"] = LogEventLevel.Warning,
                ["Error"] = LogEventLevel.Error,
                ["Fatal"] = LogEventLevel.Fatal,
                ["Verbose"] = LogEventLevel.Verbose,
            };

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.ControlledBy(new LoggingLevelSwitch
                {
                    MinimumLevel = logLevelDictionary[logEventLevel]
                })
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} <source:{SourceContext}>{NewLine}{Exception}")
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(config["AppSettings:ElasticSearchUrl"]))
                {
                    AutoRegisterTemplate = true,
                    IndexFormat = config["AppSettings:CountryCode"].ToLower() + "-process-mq-{0:yyyy.MM.dd}",
                    ConnectionTimeout = TimeSpan.FromSeconds(5),
                    DeadLetterIndexName = "td-deadletter-process-mq-{0:yyyy.MM.dd}",
                    RegisterTemplateFailure = RegisterTemplateRecovery.IndexAnyway
                })
                .CreateLogger();
        }

    }
}
