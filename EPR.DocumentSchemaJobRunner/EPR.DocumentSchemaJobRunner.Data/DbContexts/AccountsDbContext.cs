namespace EPR.DocumentSchemaJobRunner.Data.DbContexts;

using System.Diagnostics.CodeAnalysis;
using Entities;
using Microsoft.EntityFrameworkCore;

[ExcludeFromCodeCoverage]
public class AccountsDbContext : DbContext
{
    public AccountsDbContext()
    {
    }

    public AccountsDbContext(DbContextOptions<AccountsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ComplianceScheme> ComplianceSchemes { get; set; }

    public virtual DbSet<Organisation> Organisations { get; set; }
}