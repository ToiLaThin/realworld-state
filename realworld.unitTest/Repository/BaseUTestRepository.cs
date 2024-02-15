using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Realworld.Api.Data;
using Realworld.Api.Models;
using Realworld.Api.Utils;
using Realworld.UnitTest.Utils;

namespace Realworld.UnitTest.Repository
{
  public class BaseUTestRepository : IDisposable, IClassFixture<FixtureUTestRepository>
  {
    private string _connectionString = String.Empty;
    private DummyDataProvider _dummyDataProvider;
    protected IEnumerable<User> DummyUsers { get; init; }

    protected IEnumerable<Tag> DummyTags { get; init; }
    protected Realworld.Api.Data.ConduitContext ConduitContext { get; init; }

    public BaseUTestRepository(FixtureUTestRepository fixture)
    {
      //cannot add DummyDataProvider to the fixture because it needs JwtTokenGenerator, 
      //so we must dependency injection DummyDataProvider to the constructor of Fixture, 
      //but DummyDataProvider cannot be add to DI since it not live in the same assembly with Startup.cs of the project to be tested

      _connectionString = fixture.ConnectionString;

      if (_dummyDataProvider != null) //require because the constructor of UTestUserRepository will be called multiple times
        return;

      var jwtOptions = Options.Create(new JwtOptions()
      {
        Audience = "https://localhost:5001",
        Issuer = "https://localhost:5001",
        Secret = "supersecret  have to have the required length"
      });
      var jwtTokenGenerator = new JwtTokenGenerator(jwtOptions);
      _dummyDataProvider = new DummyDataProvider(jwtTokenGenerator);

      DbContextOptions<ConduitContext> dbContextOptions = new DbContextOptionsBuilder<ConduitContext>()
          .UseNpgsql(_connectionString)
          .Options;
      ConduitContext = new Realworld.Api.Data.ConduitContext(dbContextOptions);

      DummyUsers = _dummyDataProvider.GetDummyUsers();
      DummyTags = _dummyDataProvider.GetDummyTags();
    }

    public void Dispose()
    {
      ConduitContext.Users.RemoveRange(ConduitContext.Users.ToList());
      ConduitContext.Tags.RemoveRange(ConduitContext.Tags.ToList());
      ConduitContext.SaveChanges();
    }

    public virtual void SeedDb() { }
  }
}