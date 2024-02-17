using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Realworld.Api.Data;
using Testcontainers.PostgreSql;

namespace Realworld.IntegrationTest
{
  public class ConduitWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
  {
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
      .WithImage("postgres:15-alpine")
      .Build();

    public string ConnectionString {
      get {
        return _postgreSqlContainer.GetConnectionString();
      }
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
      base.ConfigureWebHost(builder);
      builder.ConfigureServices(s => {
        var descriptorType = typeof(DbContextOptions<ConduitContext>);
        var descriptor = s.SingleOrDefault(s => s.ServiceType == descriptorType);
        if (descriptor != null) {
          s.Remove(descriptor);
        }
        s.AddDbContext<ConduitContext>(
          opt => opt.UseNpgsql(_postgreSqlContainer.GetConnectionString())
        );
      });
    }
    public Task InitializeAsync()
    {
      return _postgreSqlContainer.StartAsync();
    }

    public Task DisposeAsync()
    {
      return _postgreSqlContainer.DisposeAsync().AsTask();
    }
  }
}