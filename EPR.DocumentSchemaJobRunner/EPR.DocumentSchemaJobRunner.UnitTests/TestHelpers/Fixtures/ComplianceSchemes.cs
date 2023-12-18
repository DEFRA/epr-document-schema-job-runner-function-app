namespace EPR.DocumentSchemaJobRunner.UnitTests.TestHelpers.Fixtures;

using EPR.DocumentSchemaJobRunner.Data.Entities;

public abstract class ComplianceSchemes : TestDataBase
{
    public static readonly ComplianceScheme ValpakEngland = new()
    {
        Id = 1,
        CompaniesHouseNumber = ValpakCompaniesHouseNumber,
        ExternalId = Guid.NewGuid(),
        IsDeleted = false
    };

    public static readonly ComplianceScheme ValpakScotland = new()
    {
        Id = 2,
        CompaniesHouseNumber = ValpakCompaniesHouseNumber,
        ExternalId = Guid.NewGuid(),
        IsDeleted = false
    };

    public static readonly ComplianceScheme ValpakWales = new()
    {
        Id = 3,
        CompaniesHouseNumber = ValpakCompaniesHouseNumber,
        ExternalId = Guid.NewGuid(),
        IsDeleted = true
    };

    public static readonly ComplianceScheme ValpakNorthernIreland = new()
    {
        Id = 4,
        CompaniesHouseNumber = ValpakCompaniesHouseNumber,
        ExternalId = Guid.NewGuid(),
        IsDeleted = true
    };

    public static readonly ComplianceScheme RecyclePakEngland = new()
    {
        Id = 5,
        CompaniesHouseNumber = RecyclePakCompaniesHouseNumber,
        ExternalId = Guid.NewGuid(),
        IsDeleted = false
    };
}