using System.Diagnostics.CodeAnalysis;
using Maestro.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Maestro.Data;

[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<ReminderDbo> Reminders { get; set; }
    public DbSet<ApiKeyDbo> ApiKeys { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReminderDbo>().Property(dbo => dbo.Description).HasMaxLength(DataConstraints.ReminderDescriptionMaxLength);

        modelBuilder.Entity<ApiKeyDbo>().Property(dbo => dbo.Key).HasMaxLength(32);

        // easyDebug
        modelBuilder.Entity<ApiKeyDbo>().HasData(new ApiKeyDbo
        {
            Id = 1,
            IntegratorId = 1,
            Key = "admin",
            State = ApiKeyState.Active
        });

        base.OnModelCreating(modelBuilder);
    }
}