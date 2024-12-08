using System.Diagnostics.CodeAnalysis;
using Maestro.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Maestro.Data;

[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;
    // public DbSet<Event> Events { get; set; } = null!;
}