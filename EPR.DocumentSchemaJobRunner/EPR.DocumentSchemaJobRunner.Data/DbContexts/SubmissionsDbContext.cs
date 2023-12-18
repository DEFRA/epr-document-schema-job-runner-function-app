namespace EPR.DocumentSchemaJobRunner.Data.DbContexts;

using System.Diagnostics.CodeAnalysis;
using Entities;
using Enums;
using Microsoft.EntityFrameworkCore;

[ExcludeFromCodeCoverage]
public class SubmissionsDbContext : DbContext
{
    public SubmissionsDbContext()
    {
    }

    public SubmissionsDbContext(DbContextOptions<SubmissionsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Submission> Submissions { get; set; }

    public virtual DbSet<AbstractJobOutput> JobOutputs { get; set; }

    public virtual void SetModified(object entity) => Entry(entity).State = EntityState.Modified;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Submission>(entity =>
        {
            entity.ToContainer("Submissions");
            entity.HasPartitionKey(x => x.Id);
            entity.HasNoDiscriminator();
            entity.Property(x => x.Id).ToJsonProperty("SubmissionId");
        });

        modelBuilder.Entity<AbstractJobOutput>(entity =>
        {
            entity.ToContainer("JobOutputs");
            entity.HasPartitionKey(x => x.Id);
            entity.Property(x => x.Id).ToJsonProperty("JobOutputId");
            entity.HasDiscriminator(x => x.JobType).HasValue<ComplianceSchemeIdJobOutput>(JobType.ComplianceSchemeId);
            entity.Property(x => x.JobType).HasConversion<string>();
        });

        modelBuilder.Entity<ComplianceSchemeIdJobOutput>(entity =>
        {
            entity.HasPartitionKey(x => x.Id);
        });
    }
}