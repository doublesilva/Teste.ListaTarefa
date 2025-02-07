using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using Teste.ListaTarefa.Infrastructure;

namespace Teste.ListaTarefa.IntegrationTest
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove the app's TaskDbContext registration.
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<TaskDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add TaskDbContext using an in-memory database for testing.
                services.AddDbContext<TaskDbContext>(options =>
                {
                    options.UseSqlite("Data Source=InMemoryDbForTesting.db");
                });

                // Build the service provider.
                var serviceProvider = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database context (TaskDbContext).
                using (var scope = serviceProvider.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<TaskDbContext>();
                    var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                    // Ensure the database is created.
                   // db.Database.EnsureCreated();

                    try
                    {
                        // Apply migrations.
                      //  db.Database.Migrate();
                        logger.LogInformation("Migrations applied successfully.");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred applying migrations to the database. Error: {Message}", ex.Message);
                    }
                }
            });
        }
    }
}
