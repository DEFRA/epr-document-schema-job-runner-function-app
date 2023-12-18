namespace EPR.DocumentSchemaJobRunner.Application.Jobs;

using Data.DbContexts;
using Data.Entities;
using Data.Models;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class ComplianceSchemeIdJob : IDocumentSchemaJob
{
    private readonly AccountsDbContext _accountsDbContext;
    private readonly SubmissionsDbContext _submissionsDbContext;
    private readonly ILogger<ComplianceSchemeIdJob> _logger;

    public ComplianceSchemeIdJob(
        AccountsDbContext accountsDbContext,
        SubmissionsDbContext submissionsDbContext,
        ILogger<ComplianceSchemeIdJob> logger)
    {
        _accountsDbContext = accountsDbContext;
        _submissionsDbContext = submissionsDbContext;
        _logger = logger;
    }

    public async Task RunAsync()
    {
        var startedAt = DateTime.Now;
        _logger.LogInformation("Starting {jobName}", nameof(ComplianceSchemeIdJob));

        try
        {
            var companiesHouseNumberToComplianceSchemeIdDictionary = await _accountsDbContext.ComplianceSchemes
                .Where(x => !x.IsDeleted)
                .GroupBy(x => x.CompaniesHouseNumber)
                .Select(x => x.First())
                .ToDictionaryAsync(x => x.CompaniesHouseNumber, x => x.ExternalId);

            var organisationIdToComplianceSchemeIdDictionary = await _accountsDbContext.Organisations
                .Where(x => x.IsComplianceScheme && !x.IsDeleted && companiesHouseNumberToComplianceSchemeIdDictionary.Keys.ToList().Contains(x.CompaniesHouseNumber))
                .ToDictionaryAsync(x => x.ExternalId, x => companiesHouseNumberToComplianceSchemeIdDictionary[x.CompaniesHouseNumber]);

            var organisationsIds = organisationIdToComplianceSchemeIdDictionary.Keys.ToList();
            var submissions = await _submissionsDbContext.Submissions
                .Where(x => organisationsIds.Contains(x.OrganisationId))
                .ToListAsync();

            var results = new List<ComplianceSchemeIdJobResult>();

            foreach (var submission in submissions.Where(x => x.ComplianceSchemeId is null))
            {
                var complianceSchemeId = organisationIdToComplianceSchemeIdDictionary[submission.OrganisationId];
                submission.ComplianceSchemeId = complianceSchemeId;
                results.Add(new ComplianceSchemeIdJobResult { SubmissionId = submission.Id, ComplianceSchemeId = complianceSchemeId });
                _submissionsDbContext.SetModified(submission);
            }

            await CreateSuccessJobOutputRecordAsync(startedAt, results);
            await _submissionsDbContext.SaveChangesAsync();
            _logger.LogInformation("{jobName} has run successfully", nameof(ComplianceSchemeIdJob));
        }
        catch (Exception exception)
        {
            _logger.LogCritical(exception, "{jobName} has encountered an error", nameof(ComplianceSchemeIdJob));
            await CreateFailureJobOutputRecordAsync(startedAt, new List<string> { exception.Message });
            await _submissionsDbContext.SaveChangesAsync();
        }
    }

    private async Task CreateSuccessJobOutputRecordAsync(DateTime startedAt, List<ComplianceSchemeIdJobResult> results)
    {
        await _submissionsDbContext.JobOutputs.AddAsync(new ComplianceSchemeIdJobOutput
        {
            StartedAt = startedAt,
            EndedAt = DateTime.Now,
            WasSuccessful = true,
            Results = results
        });
    }

    private async Task CreateFailureJobOutputRecordAsync(DateTime startedAt, List<string> errors)
    {
        await _submissionsDbContext.JobOutputs.AddAsync(new ComplianceSchemeIdJobOutput
        {
            StartedAt = startedAt,
            EndedAt = DateTime.Now,
            WasSuccessful = false,
            Errors = errors
        });
    }
}