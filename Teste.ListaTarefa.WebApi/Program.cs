using Teste.ListaTarefa.Application.TaskApplication;
using Teste.ListaTarefa.Infrastructure;
using Teste.ListaTarefa.Application.TaskApplication.Dtos;
using FluentValidation;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using SQLitePCL;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);



IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile($"appsettings.json", optional: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true)
    .Build();

// Initialize the SQLite provider
Batteries.Init();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});



// Configurações de Infraestrutura
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddInfrastructure(connectionString);



// CQRS
builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(CreateTaskCommandHandler).Assembly));

// Adicionar FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<TaskCreateDtoValidator>();

builder.Services.AddControllers();

// Adicionar suporte ao Swagger (se desejar)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Middleware
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();


// Configurar o Swagger (se desejar)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskListAPI V1");
});

// Aplica migrações automaticamente
using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<TaskDbContext>();
dbContext.Database.Migrate();

app.Run();
public partial class Program { }