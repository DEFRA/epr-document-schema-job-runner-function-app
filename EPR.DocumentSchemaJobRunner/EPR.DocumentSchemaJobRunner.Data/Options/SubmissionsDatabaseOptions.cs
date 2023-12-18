namespace EPR.DocumentSchemaJobRunner.Data.Options;

using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class SubmissionsDatabaseOptions
{
    public const string Section = "SubmissionsDatabase";

    public string ConnectionString { get; set; }

    public string AccountKey { get; set; }

    public string Name { get; set; }
}