using Microsoft.EntityFrameworkCore;
using TaskListAPI.Infrastructure.Data;
using Teste.ListaTarefa.Domain.Entities;
using Teste.ListaTarefa.Infrastructure.Repositories;
using Xunit;
using Task = Teste.ListaTarefa.Domain.Entities.Task;

namespace Teste.ListaTarefa.UnitTest
{  
    public class RepositorioGenericoTests
    {
        private TaskDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<TaskDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

            return new TaskDbContext(options);
        }

        [Fact]
        public async  System.Threading.Tasks.Task AddAsync_ShouldAddEntity()
        {
            var context = GetDbContext();
            var repo = new Repository<Task>(context);

            var tarefa = new Task("Test Tarefa", "Descrição da Tarefa");

            await repo.AddAsync(tarefa, default(CancellationToken));

            var result = await context.Tasks.FirstOrDefaultAsync(t => t.Title == "Test Tarefa");

            Assert.NotNull(result);
            Assert.Equal("Test Tarefa", result.Title);           
        }
    }

}