namespace EPR.DocumentSchemaJobRunner.Data.Entities;

using Enums;
using Models;

public class ComplianceSchemeIdJobOutput : AbstractJobOutput
{
    public List<ComplianceSchemeIdJobResult> Results { get; set; } = new();

    public override JobType JobType => JobType.ComplianceSchemeId;
}