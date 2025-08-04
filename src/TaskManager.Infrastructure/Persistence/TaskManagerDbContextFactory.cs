using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace TaskManager.Infrastructure
{
    public class TaskManagerDbContextFactory : IDesignTimeDbContextFactory<TaskManagerDbContext>
    {
        public TaskManagerDbContext CreateDbContext(string[] args)
        {
            // Aponta para o diretório do projeto da API para encontrar o appsettings.json
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "TaskManager.Api");
            
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<TaskManagerDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.UseNpgsql(connectionString);

            return new TaskManagerDbContext(builder.Options);
        }
    }
}