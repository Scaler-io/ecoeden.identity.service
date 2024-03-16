using IdentityServer.Configurations.App;
using Serilog.Core;
using Serilog.Events;
using ILogger = Serilog.ILogger;
using IdentityServer.Configurations.Logging;
using Serilog;
using Destructurama;

namespace IdentityServer
{
    public class Logging
    {
        public static ILogger GetLogger(IConfiguration configuration, IWebHostEnvironment environment)
        {
            var loggingOptions = configuration.GetSection("Logging").Get<LoggingConfiguration>();
            var appConfigurations = configuration.GetSection("AppConfigurations").Get<AppConfiguration>();
            var elasticUri = configuration["Elasticsearch:Uri"];
            var logIndexPattern = $"Ecoeden.IdentityServer-{environment.EnvironmentName}";

            Enum.TryParse(loggingOptions.Console.LogLevel, false, out LogEventLevel minimumEventLevel);

            var loggerConfigurations = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(new LoggingLevelSwitch(minimumEventLevel))
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithProperty(nameof(Environment.MachineName), Environment.MachineName)
                .Enrich.WithProperty(nameof(appConfigurations.ApplicationIdentifier), appConfigurations.ApplicationIdentifier)
                .Enrich.WithProperty(nameof(appConfigurations.ApplicationEnvironment), appConfigurations.ApplicationEnvironment);

            if (loggingOptions.Console.Enabled)
            {
                loggerConfigurations.WriteTo.Console(minimumEventLevel, loggingOptions.LogOutputTemplate);
            }
            if (loggingOptions.Elastic.Enabled)
            {
                loggerConfigurations.WriteTo.Elasticsearch(elasticUri, logIndexPattern);
            }

            return loggerConfigurations
                   .Destructure
                   .UsingAttributes()
                   .CreateLogger();
        }
    }
}
