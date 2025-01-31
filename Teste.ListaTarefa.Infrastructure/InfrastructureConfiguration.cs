using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Teste.ListaTarefa.Domain.Interfaces;
using Teste.ListaTarefa.Infrastructure.Repositories;

namespace Teste.ListaTarefa.Infrastructure
{
    public static class InfrastructureConfiguration
    {
        public static void AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            // Configurações do EF Core
            services.AddDbContext<TaskDbContext>(options =>
                options.UseSqlite(connectionString));

            // Repositórios
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }
    }
}
