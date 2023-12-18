namespace EPR.DocumentSchemaJobRunner.UnitTests.TestHelpers.Fixtures;

using Data.Entities;

public abstract class Organisations : TestDataBase
{
    public static readonly Organisation Valpak = new()
    {
        Id = 1,
        CompaniesHouseNumber = ValpakCompaniesHouseNumber,
        ExternalId = Guid.NewGuid(),
        IsComplianceScheme = true,
        IsDeleted = false
    };

    public static readonly Organisation RecyclePak = new()
    {
        Id = 2,
        CompaniesHouseNumber = RecyclePakCompaniesHouseNumber,
        ExternalId = Guid.NewGuid(),
        IsComplianceScheme = true,
        IsDeleted = true
    };
}