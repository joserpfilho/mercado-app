using System.Linq.Expressions;
using MercadoApp.Domain.Common;
using MercadoApp.Domain.Entities;
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

        // Filtro global de soft delete
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .HasQueryFilter(
                        Expression.Lambda(
                            Expression.Equal(
                                Expression.Property(
                                    Expression.Parameter(entityType.ClrType, "e"),
                                    nameof(ISoftDeletable.IsDeleted)),
                                Expression.Constant(false)),
                            Expression.Parameter(entityType.ClrType, "e")));
            }
        }

        base.OnModelCreating(modelBuilder);
    }
}