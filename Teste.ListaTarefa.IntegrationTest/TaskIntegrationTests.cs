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

namespace Teste.ListaTarefa.IntegrationTest
{
    public class TaskIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
 
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private readonly ILogger<TaskIntegrationTests> _logger;
        public TaskIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");

                builder.ConfigureServices(services =>
                {
                    services.AddScoped<TaskDbContext>(provider =>
                    {
                        var options = new DbContextOptionsBuilder<TaskDbContext>()
                            .UseInMemoryDatabase(databaseName: $"DBIntegration")
                            .Options;

                        return new TaskDbContext(options);
                    });

                    services.AddSingleton<ILoggerFactory>(serviceProvider =>
                    {
                        var loggerFactory = LoggerFactory.Create(config =>
                        {
                            config.AddConsole();
                            config.AddDebug();
                            config.SetMinimumLevel(LogLevel.Debug);
                        });
                        return loggerFactory;
                    });

                    services.AddLogging();
                });

            });

            _client = _factory.CreateClient();
            // Crie uma instância de ILogger utilizando o LoggerFactory
            var loggerFactory = _factory.Services.GetRequiredService<ILoggerFactory>();
            _logger = loggerFactory.CreateLogger<TaskIntegrationTests>();
            _logger.LogInformation("Initializing integration tests...");
        }

        [Fact]
        public async Task CreateTask_ShouldAddTask()
        {
            var task = new TaskCreateDto("Test Task", "Test Description");

            var response = await _client.PostAsJsonAsync("/api/tasks", task);
            response.EnsureSuccessStatusCode();

            var taskId = await response.Content.ReadFromJsonAsync<int>();
            Assert.True(taskId > 0);
        }

        [Fact]
        public async Task GetTask_ShouldReturnTask()
        {
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