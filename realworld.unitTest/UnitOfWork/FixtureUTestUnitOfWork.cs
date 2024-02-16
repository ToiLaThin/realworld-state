
using Testcontainers.PostgreSql;

namespace Realworld.UnitTest.UnitOfWork {
  public class FixtureUTestUnitOfWork : IAsyncLifetime
  {
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
        .WithImage("postgres:15-alpine")
        .Build();

    public string ConnectionString => _postgreSqlContainer.GetConnectionString();
    public Task DisposeAsync()
    {
        return _postgreSqlContainer.DisposeAsync().AsTask();
    }

    public Task InitializeAsync()
    {
        return _postgreSqlContainer.StartAsync();
    }
  }
}