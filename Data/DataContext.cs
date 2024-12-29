using System.Diagnostics.CodeAnalysis;
using Maestro.Data.Core;
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
            Key = "00000000000000000000000000000000",
            State = ApiKeyState.Active
        }, new ApiKeyDbo
        {
            Id = 2,
            IntegratorId = 2,
            Key = "ffff0000000000000000000000000000",
            State = ApiKeyState.Active
        });

        base.OnModelCreating(modelBuilder);
    }
}