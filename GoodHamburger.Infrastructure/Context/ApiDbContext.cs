using GoodHamburger.Domain.Entities;
using GoodHamburger.Infrastructure.Configuration;
using GoodHamburger.Infrastructure.Mappings;
using GoodHamburger.Infrastructure.Seeds;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Infrastructure.Context;

public class ApiDbContext : DbContext
{
    public ApiDbContext()
    {

    }

    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(ApiConfiguration.GetDatabaseConfig().ConnectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProductMap());
        modelBuilder.ApplyConfiguration(new OrderMap());
        modelBuilder.ApplyConfiguration(new OrderItemMap());

        new SeedConfig().ApplySeeds(modelBuilder);
    }

    public override int SaveChanges()
    {
        ApplyTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyTimestamps()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;

                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.DeletedAt = DateTime.UtcNow;
                    break;
            }
        }
    }
}