namespace EPR.DocumentSchemaJobRunner.Function;

using Application.Jobs.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

public class TimerTrigger
{
    private readonly IEnumerable<IDocumentSchemaJob> _documentSchemaJobs;
    private readonly ILogger<TimerTrigger> _logger;

    public TimerTrigger(IEnumerable<IDocumentSchemaJob> documentSchemaJobs, ILogger<TimerTrigger> logger)
    {
        _documentSchemaJobs = documentSchemaJobs;
        _logger = logger;
    }

    [Function("TimerTrigger")]
    public async Task RunAsync([TimerTrigger("%ScheduleExpression%", RunOnStartup = true)] TimerInfo timerInfo)
    {
        foreach (var documentSchemaJob in _documentSchemaJobs)
        {
            await documentSchemaJob.RunAsync();
        }
    }
}
