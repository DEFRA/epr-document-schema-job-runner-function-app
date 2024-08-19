namespace EPR.DocumentSchemaJobRunner.UnitTests.Application.Jobs;

using System.Linq.Expressions;
using Data.DbContexts;
using Data.Entities;
using Data.Enums;
using Data.Models;
using DocumentSchemaJobRunner.Application.Jobs;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockQueryable.Moq;
using Moq;
using TestHelpers.EqualityComparers;
using TestHelpers.Fixtures;

[TestClass]
public class ComplianceSchemeIdJobTests
{
    private Mock<ILogger<ComplianceSchemeIdJob>> _loggerMock;
    private Mock<SubmissionsDbContext> _submissionsDbContext;
    private Mock<AccountsDbContext> _accountsDbContext;
    private ComplianceSchemeIdJob _systemUnderTest;

    [TestInitialize]
    public void TestInitialize()
    {
        _loggerMock = new Mock<ILogger<ComplianceSchemeIdJob>>();
        _submissionsDbContext = new Mock<SubmissionsDbContext>();
        _accountsDbContext = new Mock<AccountsDbContext>();
        _submissionsDbContext
            .Setup(x => x.JobOutputs)
            .Returns(new List<AbstractJobOutput>().AsQueryable().BuildMockDbSet().Object);

        _systemUnderTest = new ComplianceSchemeIdJob(_accountsDbContext.Object, _submissionsDbContext.Object, _loggerMock.Object);
    }

    [TestMethod]
    public async Task RunAsync_UpdatesSubmissionsUsingOnlyFirstComplianceSchemeExternalId_WhenSubmissionsAreMissingComplianceSchemeId()
    {
        // Arrange
        var submissionOne = new Submission
        {
            Id = Guid.NewGuid(),
            OrganisationId = Organisations.Valpak.ExternalId
        };

        var submissionTwo = new Submission
        {
            Id = Guid.NewGuid(),
            OrganisationId = Organisations.Valpak.ExternalId
        };

        MockComplianceSchemesDbSet(ComplianceSchemes.ValpakEngland, ComplianceSchemes.ValpakScotland);
        MockOrganisationsDbSet(Organisations.Valpak);
        MockSubmissionsDbSet(submissionOne, submissionTwo);

        // Act
        await _systemUnderTest.RunAsync();

        // Assert
        Expression<Func<Submission, bool>> submissionOneExpectation =
            s => s.Id == submissionOne.Id
                 && s.ComplianceSchemeId == ComplianceSchemes.ValpakEngland.ExternalId;

        Expression<Func<Submission, bool>> submissionTwoExpectation =
            s => s.Id == submissionTwo.Id
                 && s.ComplianceSchemeId == ComplianceSchemes.ValpakEngland.ExternalId;

        var expectedJobResults = new List<ComplianceSchemeIdJobResult>
        {
            new()
            {
                ComplianceSchemeId = ComplianceSchemes.ValpakEngland.ExternalId,
                SubmissionId = submissionOne.Id
            },
            new()
            {
                ComplianceSchemeId = ComplianceSchemes.ValpakEngland.ExternalId,
                SubmissionId = submissionTwo.Id
            }
        };

        Expression<Func<ComplianceSchemeIdJobOutput, bool>> jobOutputExpectation =
            x => x.WasSuccessful
                 && x.JobType == JobType.ComplianceSchemeId
                 && x.Errors.Count == 0
                 && x.Results.SequenceEqual(expectedJobResults, new ComplianceSchemeIdJobResultComparer());

        _loggerMock.VerifyLog(x => x.LogInformation("Starting ComplianceSchemeIdJob"));
        _submissionsDbContext.Verify(x => x.SetModified(It.Is(submissionOneExpectation)), Times.Once);
        _submissionsDbContext.Verify(x => x.SetModified(It.Is(submissionTwoExpectation)), Times.Once);
        _submissionsDbContext.Verify(x => x.JobOutputs.AddAsync(It.Is(jobOutputExpectation), CancellationToken.None), Times.Once);
        _submissionsDbContext.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);
        _loggerMock.VerifyLog(x => x.LogInformation("ComplianceSchemeIdJob has run successfully"));
    }

    [TestMethod]
    public async Task RunAsync_DoesNotUpdateSubmissionsAndCreatesCorrectJobOutput_WhenComplianceSchemeIdIsAlreadySet()
    {
        // Arrange
        var submission = new Submission
        {
            Id = Guid.NewGuid(),
            OrganisationId = Organisations.Valpak.ExternalId,
            ComplianceSchemeId = ComplianceSchemes.ValpakEngland.ExternalId
        };

        MockComplianceSchemesDbSet(ComplianceSchemes.ValpakEngland);
        MockOrganisationsDbSet(Organisations.Valpak);
        MockSubmissionsDbSet(submission);

        // Act
        await _systemUnderTest.RunAsync();

        // Assert
        Expression<Func<ComplianceSchemeIdJobOutput, bool>> jobOutputExpectation =
            x => x.WasSuccessful
                 && x.JobType == JobType.ComplianceSchemeId
                 && x.Errors.Count == 0
                 && x.Results.Count == 0;

        _loggerMock.VerifyLog(x => x.LogInformation("Starting ComplianceSchemeIdJob"));
        _submissionsDbContext.Verify(x => x.SetModified(It.IsAny<Submission>()), Times.Never);
        _submissionsDbContext.Verify(x => x.JobOutputs.AddAsync(It.Is(jobOutputExpectation), CancellationToken.None), Times.Once);
        _submissionsDbContext.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);
        _loggerMock.VerifyLog(x => x.LogInformation("ComplianceSchemeIdJob has run successfully"));
    }

    [TestMethod]
    public async Task RunAsync_DoesNotUpdateSubmissionsAndCreatesCorrectJobOutput_WhenComplianceSchemesAreDeletedButOrganisationIsActive()
    {
        // Arrange
        var submission = new Submission
        {
            Id = Guid.NewGuid(),
            OrganisationId = Organisations.Valpak.ExternalId,
            ComplianceSchemeId = ComplianceSchemes.ValpakNorthernIreland.ExternalId
        };

        MockComplianceSchemesDbSet(ComplianceSchemes.ValpakWales, ComplianceSchemes.ValpakNorthernIreland);
        MockOrganisationsDbSet(Organisations.Valpak);
        MockSubmissionsDbSet(submission);

        // Act
        await _systemUnderTest.RunAsync();

        // Assert
        Expression<Func<ComplianceSchemeIdJobOutput, bool>> jobOutputExpectation =
            x => x.WasSuccessful
                 && x.JobType == JobType.ComplianceSchemeId
                 && x.Errors.Count == 0
                 && x.Results.Count == 0;

        _loggerMock.VerifyLog(x => x.LogInformation("Starting ComplianceSchemeIdJob"));
        _submissionsDbContext.Verify(x => x.SetModified(It.IsAny<Submission>()), Times.Never);
        _submissionsDbContext.Verify(x => x.JobOutputs.AddAsync(It.Is(jobOutputExpectation), CancellationToken.None), Times.Once);
        _submissionsDbContext.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);
        _loggerMock.VerifyLog(x => x.LogInformation("ComplianceSchemeIdJob has run successfully"));
    }

    [TestMethod]
    public async Task RunAsync_DoesNotUpdateSubmissionsAndCreatesCorrectJobOutput_WhenComplianceSchemesAreActiveButOrganisationIsDeleted()
    {
        // Arrange
        var submission = new Submission
        {
            Id = Guid.NewGuid(),
            OrganisationId = Organisations.RecyclePak.ExternalId,
            ComplianceSchemeId = ComplianceSchemes.RecyclePakEngland.ExternalId
        };

        MockComplianceSchemesDbSet(ComplianceSchemes.RecyclePakEngland);
        MockOrganisationsDbSet(Organisations.RecyclePak);
        MockSubmissionsDbSet(submission);

        // Act
        await _systemUnderTest.RunAsync();

        // Assert
        Expression<Func<ComplianceSchemeIdJobOutput, bool>> jobOutputExpectation =
            x => x.WasSuccessful
                 && x.JobType == JobType.ComplianceSchemeId
                 && x.Errors.Count == 0
                 && x.Results.Count == 0;

        _loggerMock.VerifyLog(x => x.LogInformation("Starting ComplianceSchemeIdJob"));
        _submissionsDbContext.Verify(x => x.SetModified(It.IsAny<Submission>()), Times.Never);
        _submissionsDbContext.Verify(x => x.JobOutputs.AddAsync(It.Is(jobOutputExpectation), CancellationToken.None), Times.Once);
        _submissionsDbContext.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);
        _loggerMock.VerifyLog(x => x.LogInformation("ComplianceSchemeIdJob has run successfully"));
    }

    [TestMethod]
    public async Task RunAsync_LogsAndCreatesCorrectJobOutput_WhenAnErrorHasOccurred()
    {
        // Arrange
        var submission = new Submission
        {
            Id = Guid.NewGuid(),
            OrganisationId = Organisations.Valpak.ExternalId,
            ComplianceSchemeId = ComplianceSchemes.ValpakEngland.ExternalId
        };

        MockComplianceSchemesDbSet(ComplianceSchemes.ValpakEngland);
        MockOrganisationsDbSet(Organisations.Valpak);
        MockSubmissionsDbSet(submission);

        const string exceptionMessage = "Message from the thrown exception";
        var exception = new Exception(exceptionMessage);
        _submissionsDbContext
            .SetupSequence(x => x.SaveChangesAsync(CancellationToken.None))
            .ThrowsAsync(exception)
            .Returns(Task.FromResult(1));

        // Act
        await _systemUnderTest.RunAsync();

        // Assert
        Expression<Func<ComplianceSchemeIdJobOutput, bool>> jobOutputExpectation =
            x => !x.WasSuccessful
                 && x.JobType == JobType.ComplianceSchemeId
                 && x.Errors.Count == 1
                 && x.Errors.Contains(exceptionMessage)
                 && x.Results.Count == 0;

        _loggerMock.VerifyLog(x => x.LogInformation("Starting ComplianceSchemeIdJob"));
        _submissionsDbContext.Verify(x => x.JobOutputs.AddAsync(It.Is(jobOutputExpectation), CancellationToken.None), Times.Once);
        _submissionsDbContext.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Exactly(2));
        _loggerMock.VerifyLog(x => x.LogCritical("ComplianceSchemeIdJob has encountered an error"), Times.Once);
    }

    private void MockComplianceSchemesDbSet(params ComplianceScheme[] complianceSchemes)
    {
        _accountsDbContext
            .Setup(x => x.ComplianceSchemes)
            .Returns(complianceSchemes.AsQueryable().BuildMockDbSet().Object);
    }

    private void MockOrganisationsDbSet(params Organisation[] organisations)
    {
        _accountsDbContext
            .Setup(x => x.Organisations)
            .Returns(organisations.AsQueryable().BuildMockDbSet().Object);
    }

    private void MockSubmissionsDbSet(params Submission[] submissions)
    {
        _submissionsDbContext
            .Setup(x => x.Submissions)
            .Returns(submissions.AsQueryable().BuildMockDbSet().Object);
    }
}