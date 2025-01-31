using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Teste.ListaTarefa.Domain.Entities;

namespace Teste.ListaTarefa.Infrastructure.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Text).IsRequired().HasMaxLength(500);
            builder.Property(c => c.CreatedOn).IsRequired();
            builder.Property(c => c.Deleted).IsRequired();
            builder.HasOne(c => c.Author)
                   .WithMany()
                   .HasForeignKey(c => c.AuthorId);
            builder.HasOne(c => c.Task)
                   .WithMany(t => t.Comments)
                   .HasForeignKey(c => c.TaskId);
        }
    }
}
