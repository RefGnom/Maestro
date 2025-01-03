using Maestro.Data.Core;
using Maestro.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Maestro.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<ReminderDbo> Reminders { get; set; }

    public DbSet<IntegratorApiKeyDbo> IntegratorsApiKeys { get; set; }

    public DbSet<IntegratorPolicyDbo> IntegratorsPoliciesDbo { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReminderDbo>().Property(dbo => dbo.Description).HasMaxLength(DataConstraints.ReminderDescriptionMaxLength);

        modelBuilder.Entity<IntegratorApiKeyDbo>().Property(dbo => dbo.ApiKey).HasMaxLength(DataConstraints.ApiKeyHashMaxLength);
        modelBuilder.Entity<IntegratorApiKeyDbo>().HasIndex(dbo => dbo.ApiKey).IsUnique();

        modelBuilder.Entity<IntegratorPolicyDbo>().Property(dbo => dbo.Policy).HasMaxLength(16);
        modelBuilder.Entity<IntegratorPolicyDbo>().HasAlternateKey(dbo => new { dbo.IntegratorId, dbo.Policy });

        modelBuilder.Entity<IntegratorApiKeyDbo>().HasData(
            new IntegratorApiKeyDbo
            {
                Id = 1,
                IntegratorId = 1,
                ApiKey = "<no-key>",
                State = ApiKeyState.Inactive
            });

        modelBuilder.Entity<IntegratorPolicyDbo>().HasData(
            new IntegratorPolicyDbo
            {
                Id = 1,
                IntegratorId = 1,
                Policy = "<no-policy>"
            });

        base.OnModelCreating(modelBuilder);
    }
}