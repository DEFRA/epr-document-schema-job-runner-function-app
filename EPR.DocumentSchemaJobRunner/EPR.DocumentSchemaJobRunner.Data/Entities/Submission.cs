namespace EPR.DocumentSchemaJobRunner.Data.Entities;

public class Submission
{
    public Guid Id { get; init; }

    public Guid OrganisationId { get; init; }

    public Guid? ComplianceSchemeId { get; set; }
}