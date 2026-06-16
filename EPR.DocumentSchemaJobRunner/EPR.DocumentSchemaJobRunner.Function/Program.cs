namespace EPR.DocumentSchemaJobRunner.Function;

using System.Diagnostics.CodeAnalysis;
using Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

[ExcludeFromCodeCoverage]
public static class Program
{
    public static void Main(string[] args)
    {
        var builder = FunctionsApplication.CreateBuilder(args);

        builder.Services.AddApplicationInsightsTelemetryWorkerService();
        builder.Services.ConfigureFunctionsApplicationInsights();

        builder.Logging.Services.Configure<LoggerFilterOptions>(options =>
        {
            var defaultRule = options.Rules.FirstOrDefault(rule =>
                rule.ProviderName == "Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider");
            if (defaultRule is not null)
            {
                options.Rules.Remove(defaultRule);
            }
        });

        builder.Services.RegisterOptions(builder.Configuration);
        builder.Services.RegisterDatabases();
        builder.Services.RegisterJobs();

        builder.Build().Run();
    }
}