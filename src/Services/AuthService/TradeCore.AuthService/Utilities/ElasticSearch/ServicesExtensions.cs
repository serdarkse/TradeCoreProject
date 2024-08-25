using Serilog;
using Serilog.Events;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;
using System.Diagnostics;

namespace TradeCore.AuthService.Utilities.ElasticSearch
{
    public static class ServicesExtensions
    {
        public static void ConfigurElasticLoggingService(this IServiceCollection services)
        {
            ConfigureElasticLogging();
        }

        private static void ConfigureElasticLogging()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration =
                new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
                .Build();

            Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithEnvironmentName()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .MinimumLevel.Override("SerilogElasticCustomField.Behaviors.LoggingBehavior", LogEventLevel.Warning)
            .WriteTo.Debug()
            .WriteTo.Console()
            .WriteTo.Logger(lc => lc.Filter.ByExcluding(logEvent =>
                                logEvent.Properties.ContainsKey("SourceContext")
                                && logEvent.Properties["SourceContext"].ToString().Contains("Microsoft.EntityFrameworkCore")
                                && logEvent.Level != LogEventLevel.Error)
                                .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment!))
                            )
            .Enrich.WithProperty("Environment", environment!)
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        }

        private static ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration, string environment)
        {
            Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));

            var uriArray = configuration.GetSection("ElasticConfiguration:Uri").Value.Split(",");
            var uriList = new List<Uri>();
            foreach (var item in uriArray)
                uriList.Add(new Uri(item));

            var userName = configuration.GetSection("ElasticConfiguration:Username").Value;
            var password = configuration.GetSection("ElasticConfiguration:Password").Value;
            var indexFormat = configuration.GetSection("ElasticConfiguration:IndexFormat").Value;

            ElasticsearchSinkOptions options = new ElasticsearchSinkOptions(uriList)
            {
                AutoRegisterTemplate = true,
                ModifyConnectionSettings = (c) => c
                    .BasicAuthentication(userName, password)
                    .ServerCertificateValidationCallback((o, certificate, arg3, arg4) => { return true; }),

                CustomFormatter = new ElasticsearchJsonFormatter(renderMessage: true),
                IndexDecider = (logEvent, dateTimeOffset) =>
                {
                    return $"{indexFormat}-{dateTimeOffset:yyyy-MM-dd}".ToLower();
                }
            };

            return options;
        }


    }
}
