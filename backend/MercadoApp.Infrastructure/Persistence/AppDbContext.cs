using System.Linq.Expressions;
using MercadoApp.Domain.Common;
using MercadoApp.Domain.Entities;
using MercadoApp.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace MercadoApp.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<GroupMember> GroupMembers => Set<GroupMember>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Item> Items => Set<Item>();
    public DbSet<ShoppingList> ShoppingLists => Set<ShoppingList>();
    public DbSet<ListItem> ListItems => Set<ListItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
            {
                var method = typeof(AppDbContext)
                    .GetMethod(nameof(GetSoftDeleteFilter), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
                    .MakeGenericMethod(entityType.ClrType);

                var filter = method.Invoke(null, null);
                entityType.SetQueryFilter((LambdaExpression)filter!);
            }
        }

        base.OnModelCreating(modelBuilder);
    }

    private static LambdaExpression GetSoftDeleteFilter<TEntity>() where TEntity : class, ISoftDeletable
    {
        Expression<Func<TEntity, bool>> filter = e => !e.IsDeleted;
        return filter;
    }
}