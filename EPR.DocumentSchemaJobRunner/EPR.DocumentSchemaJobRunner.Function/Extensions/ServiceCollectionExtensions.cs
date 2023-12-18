namespace EPR.DocumentSchemaJobRunner.Function.Extensions;

using System.Diagnostics.CodeAnalysis;
using Application.Jobs;
using Application.Jobs.Interfaces;
using Data.DbContexts;
using Data.Options;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static void RegisterOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AccountsDatabaseOptions>(configuration.GetSection(AccountsDatabaseOptions.Section));
        services.Configure<SubmissionsDatabaseOptions>(configuration.GetSection(SubmissionsDatabaseOptions.Section));
    }

    public static void RegisterDatabases(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();

        RegisterAccountsDatabase(services, serviceProvider);
        RegisterCosmosDatabase(services, serviceProvider);
    }

    public static void RegisterJobs(this IServiceCollection services)
    {
        services.AddScoped<IDocumentSchemaJob, ComplianceSchemeIdJob>();
    }

    private static void RegisterAccountsDatabase(IServiceCollection services, IServiceProvider serviceProvider)
    {
        var databaseOptions = serviceProvider.GetRequiredService<IOptions<AccountsDatabaseOptions>>().Value;
        services.AddDbContext<AccountsDbContext>(options => options.UseSqlServer(databaseOptions.ConnectionString));
    }

    private static void RegisterCosmosDatabase(IServiceCollection services, IServiceProvider serviceProvider)
    {
        var databaseOptions = serviceProvider.GetRequiredService<IOptions<SubmissionsDatabaseOptions>>().Value;
        services.AddDbContext<SubmissionsDbContext>(options => options.UseCosmos(
            databaseOptions.ConnectionString,
            databaseOptions.AccountKey,
            databaseOptions.Name,
            cosmosOptions => cosmosOptions.ConnectionMode(ConnectionMode.Gateway)));
    }
}