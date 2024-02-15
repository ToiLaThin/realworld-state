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

    protected IEnumerable<Article> DummyArticles { get; init; }

    protected IEnumerable<Comment> DummyComments { get; init; }
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
          .EnableSensitiveDataLogging() //for viewing conflict key error in change tracking
          .Options;
      ConduitContext = new Realworld.Api.Data.ConduitContext(dbContextOptions);

      DummyUsers = _dummyDataProvider.GetDummyUsers();
      DummyTags = _dummyDataProvider.GetDummyTags();
      DummyArticles = _dummyDataProvider.GetDummyArticles();
      DummyComments = _dummyDataProvider.GetDummyComments();
    }

    public void Dispose()
    {
      var transaction = ConduitContext.Database.BeginTransaction();
      ConduitContext.Users.RemoveRange(ConduitContext.Users.ToList());
      ConduitContext.Articles.RemoveRange(ConduitContext.Articles.ToList());
      ConduitContext.Tags.RemoveRange(ConduitContext.Tags.ToList());
      ConduitContext.SaveChanges();
      transaction.Commit();
    }

    public void SeedDb()
    {
      bool isDbJustCreated = ConduitContext.Database.EnsureCreated();
      var transaction = ConduitContext.Database.BeginTransaction();
      if (ConduitContext.Tags.Any()) return;
      foreach (var tag in DummyTags) {
        ConduitContext.Entry(tag).State = EntityState.Added;
      }

      if (ConduitContext.Users.Any()) return;
      foreach (var user in DummyUsers) {
        ConduitContext.Entry(user).State = EntityState.Added; //should do it this way to avoid entity already tracked error
      }

      if (ConduitContext.Articles.Any()) return;
      foreach (var article in DummyArticles)
      {
        ConduitContext.Entry(article).State = EntityState.Added;
      }

      if (ConduitContext.Comments.Any()) return;
      ConduitContext.Comments.AddRange(DummyComments);

      ConduitContext.SaveChanges();
      transaction.Commit();
    }
  }
}