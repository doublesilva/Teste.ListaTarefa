using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using Teste.ListaTarefa.Application.TaskApplication.Dtos;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Teste.ListaTarefa.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore.Sqlite.Infrastructure.Internal;

namespace Teste.ListaTarefa.IntegrationTest
{
    public class TaskIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>, IAsyncLifetime
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private readonly IServiceScope _scope;
        private TaskDbContext _context;
        private DbContextOptions<TaskDbContext> _options;

        public TaskIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();

            _scope = _factory.Services.CreateScope();
            _context = _scope.ServiceProvider.GetRequiredService<TaskDbContext>();

            _options = new DbContextOptionsBuilder<TaskDbContext>()
                .UseSqlite("Data Source=:memory:")
                .Options;
        }
        public async Task InitializeAsync()
        {
            await InitializeDatabaseAsync();
        }

        public Task DisposeAsync()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
            _scope.Dispose();
            return Task.CompletedTask;
        }

        private async Task InitializeDatabaseAsync()
        {
            await CleanupDatabaseAsync();

            using (var context = new TaskDbContext(_options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();
                context.Database.Migrate();
            }

            await VerifyMigrationsApplied();
        }

        private async Task CleanupDatabaseAsync()
        {
            using (var context = new TaskDbContext(_options))
            {
                await context.Database.EnsureDeletedAsync();
            }
        }

        private async Task VerifyMigrationsApplied()
        {
            var appliedMigrations = await _context.Database.GetAppliedMigrationsAsync();
            var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();

            Assert.Empty(pendingMigrations);
            Assert.NotEmpty(appliedMigrations);

        }

        [Fact]
        public async Task CreateTask_ShouldAddTask()
        {
            await InitializeDatabaseAsync();
            var task = new TaskCreateDto("Test Task", "Test Description");

            var response = await _client.PostAsJsonAsync("/api/tasks", task);
            response.EnsureSuccessStatusCode();

            var taskId = await response.Content.ReadFromJsonAsync<int>();
            Assert.True(taskId > 0);
        }

        [Fact]
        public async Task GetTask_ShouldReturnTask()
        {
            await InitializeDatabaseAsync();
            var task = new TaskCreateDto("Test Task", "Test Description");

            var createResponse = await _client.PostAsJsonAsync("/api/tasks", task);
            createResponse.EnsureSuccessStatusCode();

            var taskId = await createResponse.Content.ReadFromJsonAsync<int>();
            var response = await _client.GetAsync($"/api/tasks/{taskId}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<TaskQueryDto>();
            Assert.NotNull(result);
            Assert.Equal("Test Task", result.Title);
        }

        [Fact]
        public async Task UpdateTask_ShouldModifyTask()
        {
            await InitializeDatabaseAsync();
            var task = new TaskCreateDto("Test Task", "Test Description");

            var createResponse = await _client.PostAsJsonAsync("/api/tasks", task);
            createResponse.EnsureSuccessStatusCode();

            var taskId = await createResponse.Content.ReadFromJsonAsync<int>();
            var updateTask = new TaskUpdateDto("Updated Task", "Updated Description",
                null, null, null);


            var response = await _client.PutAsJsonAsync($"/api/tasks/{taskId}", updateTask);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<bool>();
            Assert.True(result);

            var getResponse = await _client.GetAsync($"/api/tasks/{taskId}");
            getResponse.EnsureSuccessStatusCode();

            var updatedTask = await getResponse.Content.ReadFromJsonAsync<TaskQueryDto>();
            Assert.NotNull(updatedTask);
            Assert.Equal("Updated Task", updatedTask.Title);
        }

        [Fact]
        public async Task DeleteTask_ShouldRemoveTask()
        {
            await InitializeDatabaseAsync();
            var task = new TaskCreateDto("Test Task", "Test Description");

            var createResponse = await _client.PostAsJsonAsync("/api/tasks", task);
            createResponse.EnsureSuccessStatusCode();

            var taskId = await createResponse.Content.ReadFromJsonAsync<int>();

            var deleteResponse = await _client.DeleteAsync($"/api/tasks/{taskId}");
            deleteResponse.EnsureSuccessStatusCode();

            var result = await deleteResponse.Content.ReadFromJsonAsync<bool>();
            Assert.True(result);

            var getResponse = await _client.GetAsync($"/api/tasks/{taskId}");
            Assert.Equal(System.Net.HttpStatusCode.NotFound, getResponse.StatusCode);
        }

        [Fact]
        public async Task GetAllTasks_ShouldReturnAllTasks()
        {
            await InitializeDatabaseAsync();
            var task1 = new TaskCreateDto("Test Task 1", "Test Description 1");

            var task2 = new TaskCreateDto("Test Task 2", "Test Description 2");

            await _client.PostAsJsonAsync("/api/tasks", task1);
            await _client.PostAsJsonAsync("/api/tasks", task2);

            var response = await _client.GetAsync("/api/tasks");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<List<TaskQueryDto>>();
            Assert.NotNull(result);
            Assert.True(result.Count >= 2);
        }
    }
}