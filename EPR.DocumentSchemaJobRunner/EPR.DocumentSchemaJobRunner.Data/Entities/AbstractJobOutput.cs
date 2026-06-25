namespace EPR.DocumentSchemaJobRunner.Data.Entities;

using Enums;

public abstract class AbstractJobOutput
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime StartedAt { get; set; }

    public DateTime EndedAt { get; set; }

    public JobType JobType { get; set; }

    public bool WasSuccessful { get; set; }

    public List<string> Errors { get; set; } = new ();
}