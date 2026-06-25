namespace EPR.DocumentSchemaJobRunner.Data.Entities;

using Enums;
using Models;

public class ComplianceSchemeIdJobOutput : AbstractJobOutput
{
    public ComplianceSchemeIdJobOutput()
    {
        JobType = JobType.ComplianceSchemeId;
    }

    public List<ComplianceSchemeIdJobResult> Results { get; set; } = new();
}