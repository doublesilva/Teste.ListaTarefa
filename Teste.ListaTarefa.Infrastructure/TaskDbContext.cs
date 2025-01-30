using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Teste.ListaTarefa.Domain.Entities;
using Teste.ListaTarefa.Infrastructure.Configurations;
using Task = Teste.ListaTarefa.Domain.Entities.Task;

namespace TaskListAPI.Infrastructure.Data
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options) { }

        public DbSet<Task> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Aplicar configurações de entidades
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new TaskConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());

            // Aplicar filtro global para soft delete
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(GetNotDeletedFilter(entityType.ClrType));
                }
            }

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            UpdateEntities();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            UpdateEntities();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void UpdateEntities()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedOn = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedOn = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.Entity.Deleted = true;
                }
            }
        }

        private static LambdaExpression GetNotDeletedFilter(Type entityType)
        {
            var parameter = Expression.Parameter(entityType, "entity");
            var property = Expression.Property(parameter, nameof(BaseEntity.Deleted));
            var condition = Expression.Equal(property, Expression.Constant(false));
            return Expression.Lambda(condition, parameter);
        }
    }
}
