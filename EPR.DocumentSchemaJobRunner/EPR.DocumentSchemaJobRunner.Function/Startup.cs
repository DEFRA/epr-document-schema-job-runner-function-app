using System.Diagnostics.CodeAnalysis;
using EPR.DocumentSchemaJobRunner.Function;
using EPR.DocumentSchemaJobRunner.Function.Extensions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]

namespace EPR.DocumentSchemaJobRunner.Function;

[ExcludeFromCodeCoverage]
public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.GetContext().Configuration;

        services.RegisterOptions(configuration);
        services.RegisterDatabases();
        services.RegisterJobs();
    }
}