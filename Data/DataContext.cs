using Maestro.Data.Core;
using Maestro.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Maestro.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<ReminderDbo> Reminders { get; set; }

    public DbSet<IntegratorApiKeyDbo> IntegratorsApiKeys { get; set; }

    public DbSet<IntegratorRoleDbo> IntegratorsRolesDbo { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReminderDbo>().Property(dbo => dbo.Description).HasMaxLength(DataConstraints.DescriptionMaxLength);

        modelBuilder.Entity<IntegratorApiKeyDbo>().Property(dbo => dbo.ApiKey).HasMaxLength(DataConstraints.ApiKeyHashMaxLength);
        modelBuilder.Entity<IntegratorApiKeyDbo>().HasIndex(dbo => dbo.ApiKey).IsUnique();

        modelBuilder.Entity<IntegratorRoleDbo>().Property(dbo => dbo.Role).HasMaxLength(16);
        modelBuilder.Entity<IntegratorRoleDbo>().HasAlternateKey(dbo => new { dbo.IntegratorId, dbo.Role });

        modelBuilder.Entity<IntegratorApiKeyDbo>().HasData(
            new IntegratorApiKeyDbo
            {
                Id = 1,
                IntegratorId = 1,
                ApiKey = "<no-key>",
                State = ApiKeyState.Inactive
            });

        modelBuilder.Entity<IntegratorRoleDbo>().HasData(
            new IntegratorRoleDbo
            {
                Id = 1,
                IntegratorId = 1,
                Role = "<no-role>"
            });

        base.OnModelCreating(modelBuilder);
    }
}