using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teste.ListaTarefa.Domain.Entities;

namespace Teste.ListaTarefa.Infrastructure.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Name).IsRequired().HasMaxLength(100);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(100);
            builder.Property(u => u.CreatedOn).IsRequired();
            builder.Property(u => u.Deleted).IsRequired();
            builder.HasData(
               new User
               {
                   Id = 1,
                   Name = "Diego",
                   Email = "silva.pcrs@asda.com"
               }
           );
        }
    }
}
