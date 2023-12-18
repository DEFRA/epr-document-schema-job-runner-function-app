namespace EPR.DocumentSchemaJobRunner.UnitTests.TestHelpers.EqualityComparers;

using EPR.DocumentSchemaJobRunner.Data.Models;

public class ComplianceSchemeIdJobResultComparer : IEqualityComparer<ComplianceSchemeIdJobResult>
{
    public bool Equals(ComplianceSchemeIdJobResult? x, ComplianceSchemeIdJobResult? y)
    {
        if (x == null && y == null)
        {
            return true;
        }

        if (x == null || y == null)
        {
            return false;
        }

        return x.ComplianceSchemeId == y.ComplianceSchemeId && x.SubmissionId == y.SubmissionId;
    }

    public int GetHashCode(ComplianceSchemeIdJobResult obj)
    {
        return HashCode.Combine(obj.ComplianceSchemeId, obj.SubmissionId);
    }
}