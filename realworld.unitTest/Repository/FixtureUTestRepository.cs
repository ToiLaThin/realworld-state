using Realworld.UnitTest.Utils;
using Testcontainers.PostgreSql;

namespace Realworld.UnitTest.Repository
{
    public class FixtureUTestRepository : IAsyncLifetime
    {
        private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
            .WithImage("postgres:15-alpine")
            .Build();

        public string ConnectionString => _postgreSqlContainer.GetConnectionString();
        
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
