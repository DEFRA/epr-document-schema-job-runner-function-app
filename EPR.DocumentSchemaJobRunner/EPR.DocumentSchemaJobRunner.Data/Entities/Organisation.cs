namespace EPR.DocumentSchemaJobRunner.Data.Entities;

public class Organisation
{
    public int Id { get; init; }

    public Guid ExternalId { get; init; }

    public string? CompaniesHouseNumber { get; init; }

    public bool IsComplianceScheme { get; init; }

    public bool IsDeleted { get; init; }
}