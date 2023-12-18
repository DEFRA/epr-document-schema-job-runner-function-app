namespace EPR.DocumentSchemaJobRunner.Data.Entities;

public class ComplianceScheme
{
    public int Id { get; init; }

    public Guid ExternalId { get; init; }

    public string CompaniesHouseNumber { get; init; }

    public bool IsDeleted { get; init; }
}