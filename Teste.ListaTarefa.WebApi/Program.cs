using Teste.ListaTarefa.Application.TaskApplication;
using Teste.ListaTarefa.Infrastructure;
using Teste.ListaTarefa.Application.TaskApplication.Dtos;
using FluentValidation;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;


var builder = WebApplication.CreateBuilder(args);


IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile($"appsettings.json", optional: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true)
    .Build();


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

app.Run();
public partial class Program { }