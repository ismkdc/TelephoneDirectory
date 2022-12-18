using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace TelephoneDirectory.Data.Entities;

public class TelephoneDirectoryContext : DbContext
{
    public TelephoneDirectoryContext(DbContextOptions<TelephoneDirectoryContext> options)
        : base(options)
    {
    }

    public TelephoneDirectoryContext()
    {
    }

    public virtual DbSet<Contact> Contacts { get; set; }
    public virtual DbSet<ContactInformation> ContactInformation { get; set; }
    public virtual DbSet<Report> Reports { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // set global filter to all entities for soft delete
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var isDeletedProperty = entityType.FindProperty("DeletedAt");

            if (isDeletedProperty != null)
            {
                var parameter = Expression.Parameter(entityType.ClrType, "p");
                var filter = Expression.Lambda(
                    Expression.Equal(
                        Expression.Property(parameter, isDeletedProperty.PropertyInfo),
                        Expression.Constant(null, typeof(DateTime?))
                    ), parameter);

                entityType.SetQueryFilter(filter);
            }
        }
    }


    public override Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default
    )
    {
        var date = DateTime.UtcNow;
        var addedEntities = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.State == EntityState.Added)
            .ToList();

        addedEntities.ForEach(e => { e.Entity.CreatedAt = date; });

        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}