using System.Reflection;
using FeatureFlagsEfDemo.Features.FeatureFlags.Entities;
using Microsoft.EntityFrameworkCore;

namespace FeatureFlagsEfDemo.Data;

public interface IApplicationDbContext : IDisposable
{
    DbSet<FeatureEntity> Features { get; set; }
    DbSet<FeatureDetailEntity> FeatureDetails { get; set; }

    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken token = default);
}

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IApplicationDbContext
{
    public virtual DbSet<FeatureEntity> Features { get; set; }
    public virtual DbSet<FeatureDetailEntity> FeatureDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}