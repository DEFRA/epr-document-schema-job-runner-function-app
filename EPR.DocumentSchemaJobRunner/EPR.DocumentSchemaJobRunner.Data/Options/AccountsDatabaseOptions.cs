namespace EPR.DocumentSchemaJobRunner.Data.Options;

using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class AccountsDatabaseOptions
{
    public const string Section = "AccountsDatabase";

    public string ConnectionString { get; set; }
}