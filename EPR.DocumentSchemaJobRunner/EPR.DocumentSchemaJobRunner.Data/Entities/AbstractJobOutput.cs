namespace EPR.DocumentSchemaJobRunner.Data.Entities;

using Enums;

public abstract class AbstractJobOutput
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime StartedAt { get; set; }

    public DateTime EndedAt { get; set; }

    public virtual JobType JobType { get; }

    public bool WasSuccessful { get; set; }

    public List<string> Errors { get; set; } = new ();
}