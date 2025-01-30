using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Teste.ListaTarefa.Application.TaskApplication.Dtos;
using Xunit;

namespace Teste.ListaTarefa.E2ETest
{
   

    public class TasksControllerE2ETests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private readonly IWebDriver _driver;

        public TasksControllerE2ETests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            _driver = new ChromeDriver();
        }

        [Fact]
        public async Task GetTasks_ShouldReturnTasks()
        {
            var response = await _client.GetAsync("/api/tasks");
            response.EnsureSuccessStatusCode();

            var tasks = await response.Content.ReadFromJsonAsync<List<TaskQueryDto>>();
            Assert.NotNull(tasks);
        }

        [Fact]
        public async Task CreateTask_ShouldCreateTask()
        {
            var newTask = new TaskCreateDto("Nova Tarefa", "Descrição da Nova Tarefa");

            var response = await _client.PostAsJsonAsync("/api/tasks", newTask);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<bool>();
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateTask_ShouldUpdateTask()
        {
            var updateTask = new TaskUpdateDto("Tarefa Atualizada","Descrição Atualizada", null, null, null);

            var response = await _client.PutAsJsonAsync("/api/tasks/1", updateTask);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<bool>();
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteTask_ShouldDeleteTask()
        {
            var response = await _client.DeleteAsync("/api/tasks/1");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<bool>();
            Assert.True(result);
        }

        [Fact]
        public async Task StartTask_ShouldStartTask()
        {
            var response = await _client.PatchAsync("/api/tasks/start/1/to/2", null);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<bool>();
            Assert.True(result);
        }

        [Fact]
        public async Task FinishTask_ShouldFinishTask()
        {
            var response = await _client.PatchAsync("/api/tasks/finish/1", null);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<bool>();
            Assert.True(result);
        }

        public void Dispose()
        {
            _client.Dispose();
            _driver.Quit();
            _driver.Dispose();
        }
    }

}