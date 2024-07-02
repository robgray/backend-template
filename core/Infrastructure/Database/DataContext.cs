namespace Core.Infrastructure.Database;

using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Core.Domain.Models;
using Core.Infrastructure.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Serilog;

public class DataContext : DbContext
{
    private readonly TimeProvider _timeProvider;
    private readonly IUserContext _userContext;
    
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        /*
         * This is required because DataContext is registered with the IoC container
         * using AddDbContextPool.  Pooled DbContexts can only have one constructor
         * parameter and that parameter must be DbContextOptions<T>.
         *
         * Furthermore, you need to register the service provider with the options builder
         *    optionsBuilder.UseInternalServiceProvider(serviceProvider);
         *
         * See DbContextStartup.AddCustomDbContext for implementation.
         */
        _timeProvider = this.GetService<TimeProvider>();
        _userContext = this.GetService<IUserContext>();
    }
    
    public DbSet<Example> Examples { get; set; }

    public override int SaveChanges()
    {
        SetAutomaticModel();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        SetAutomaticModel();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        SetAutomaticModel();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetAutomaticModel();
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

    private void SetAutomaticModel()
    {
        var modifiedEntries = ChangeTracker.Entries()
            .Where(x => x.State is EntityState.Added or EntityState.Modified or EntityState.Deleted);

        var now = _timeProvider.GetUtcNow();
        
        foreach (var entry in modifiedEntries)
        {
            if (entry.Entity is IAuditable modifiedEntity)
            {
                modifiedEntity.LastModifiedBy = _userContext.Id;
                modifiedEntity.LastModifiedAt = now;

                if (entry.State == EntityState.Added)
                {
                    modifiedEntity.CreatedBy = _userContext.Id;
                    modifiedEntity.CreatedAt = now;
                }
            }
        }
    }
}
