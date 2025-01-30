using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Teste.ListaTarefa.Domain.Entities;
using Task = Teste.ListaTarefa.Domain.Entities.Task;


namespace Teste.ListaTarefa.Infrastructure.Configurations
{
    public class TaskConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Status).IsRequired().HasMaxLength(50);
            builder.Property(t => t.CreatedOn).IsRequired();
            builder.Property(t => t.Deleted).IsRequired();
            builder.HasOne(t => t.Author)
                   .WithMany()
                   .HasForeignKey(t => t.AuthorId);
            builder.HasOne(t => t.Owner)
                   .WithMany()
                   .HasForeignKey(t => t.OwnerId);
        }
    }
}
