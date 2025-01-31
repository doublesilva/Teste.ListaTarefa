using Microsoft.EntityFrameworkCore;
using Teste.ListaTarefa.Infrastructure;
using Teste.ListaTarefa.Infrastructure.Repositories;
using Task = Teste.ListaTarefa.Domain.Entities.Task;

namespace Teste.ListaTarefa.UnitTest
{
    public class GenericRepositoryTests
    {
        private TaskDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<TaskDbContext>()
                .UseInMemoryDatabase(databaseName: $"DB:{Guid.NewGuid().ToString()}")
            .Options;
            return new TaskDbContext(options);
        }

        [Fact]
        public async System.Threading.Tasks.Task AddAsync_ShouldAddEntity()
        {
            var context = GetDbContext();
            var repo = new Repository<Task>(context);

            var tarefa = new Task("Test Tarefa", "Descrição da Tarefa");

            await repo.AddAsync(tarefa, default(CancellationToken));

            var result = await context.Tasks.FirstOrDefaultAsync(t => t.Title == "Test Tarefa");

            Assert.NotNull(result);
            Assert.Equal("Test Tarefa", result.Title);
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateTask_ShouldAddTask()
        {
            var context = GetDbContext();
            var repo = new Repository<Task>(context);

            var task = new Task("Test Task", "Test Description");
            task.SetInfo(null, DateTime.Now, DateTime.Now.AddDays(2));

            await repo.AddAsync(task, default(CancellationToken));
            var result = await context.Tasks.FirstOrDefaultAsync(t => t.Title == "Test Task");

            Assert.NotNull(result);
            Assert.Equal("Test Task", result.Title);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTask_ShouldReturnTask()
        {
            var context = GetDbContext();
            var repo = new Repository<Task>(context);

            var task = new Task("Test Task", "Test Description");
            task.SetInfo(null, DateTime.Now, DateTime.Now.AddDays(2));
            await repo.AddAsync(task, default(CancellationToken));
            var result = await repo.GetByIdAsync(task.Id, default(CancellationToken));

            Assert.NotNull(result);
            Assert.Equal("Test Task", result.Title);
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateTask_ShouldModifyTask()
        {
            var context = GetDbContext();
            var repo = new Repository<Task>(context);

            var task = new Task("Test Task", "Test Description");

            await repo.AddAsync(task, default(CancellationToken));
            task.SetTitle("Updated Task");
            await repo.UpdateAsync(task, default(CancellationToken));
            var result = await context.Tasks.FirstOrDefaultAsync(t => t.Id == task.Id);

            Assert.NotNull(result);
            Assert.Equal("Updated Task", result.Title);
        }

        [Fact]
        public async System.Threading.Tasks.Task DeleteTask_ShouldRemoveTask()
        {
            var context = GetDbContext();
            var repo = new Repository<Task>(context);

            var task = new Task("Test Task", "Test Description");

            await repo.AddAsync(task, default(CancellationToken));
            await repo.DeleteAsync(task.Id, default(CancellationToken));
            var result = await context.Tasks.FirstOrDefaultAsync(t => t.Id == task.Id);

            Assert.Null(result);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetAllTasks_ShouldReturnAllTasks()
        {
            var context = GetDbContext();
            var repo = new Repository<Task>(context);

            var task1 = new Task("Test Task 1", "Test Description 1");
            var task2 = new Task("Test Task 2", "Test Description 2");

            await repo.AddAsync(task1, default(CancellationToken));
            await repo.AddAsync(task2, default(CancellationToken));
            var result = await repo.GetAllAsync(default(CancellationToken));

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
    }

}