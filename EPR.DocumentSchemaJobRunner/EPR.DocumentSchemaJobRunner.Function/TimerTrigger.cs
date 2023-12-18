namespace EPR.DocumentSchemaJobRunner.Function;

using Application.Jobs.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

public class TimerTrigger
{
    private readonly IEnumerable<IDocumentSchemaJob> _documentSchemaJobs;

    public TimerTrigger(IEnumerable<IDocumentSchemaJob> documentSchemaJobs)
    {
        _documentSchemaJobs = documentSchemaJobs;
    }

    [FunctionName("TimerTrigger")]
    public async Task RunAsync([TimerTrigger("%ScheduleExpression%", RunOnStartup = true)] TimerInfo timerInfo, ILogger log)
    {
        foreach (var documentSchemaJob in _documentSchemaJobs)
        {
            await documentSchemaJob.RunAsync();
        }
    }
}