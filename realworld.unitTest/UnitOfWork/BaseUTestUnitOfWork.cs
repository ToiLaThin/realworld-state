using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Realworld.Api.Data;
using Realworld.Api.Models;
using Realworld.Api.Utils;
using Realworld.UnitTest.Utils;

namespace Realworld.UnitTest.UnitOfWork
{
    public class BaseUTestUnitOfWork : IClassFixture<FixtureUTestUnitOfWork>
    {
        private string _connectionString = String.Empty;

        private DummyDataProvider _dummyDataProvider;

        protected IEnumerable<User> DummyUsers { get; init; }

        protected IEnumerable<Tag> DummyTags { get; init; }

        protected IEnumerable<Article> DummyArticles { get; init; }

        protected IEnumerable<Comment> DummyComments { get; init; }

        protected Realworld.Api.Data.ConduitContext ConduitContext { get; init; }

        public BaseUTestUnitOfWork(FixtureUTestUnitOfWork fixture)
        {
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
            DummyUsers = _dummyDataProvider.GetDummyUsers();
            DummyTags = _dummyDataProvider.GetDummyTags();
            DummyArticles = _dummyDataProvider.GetDummyArticles();
            DummyComments = _dummyDataProvider.GetDummyComments();


            _connectionString = fixture.ConnectionString;
            DbContextOptions<ConduitContext> dbContextOptions = new DbContextOptionsBuilder<ConduitContext>()
                .UseNpgsql(_connectionString)
                .EnableSensitiveDataLogging() //for viewing conflict key error in change tracking
                .Options;
            ConduitContext = new Realworld.Api.Data.ConduitContext(dbContextOptions);
        }       

        /// <summary>
        /// Seed the database with dummy data, call this method in inherited test class
        /// </summary>
        public void SeedDb()
        {
            bool isDbJustCreated = ConduitContext.Database.EnsureCreated();
            var transaction = ConduitContext.Database.BeginTransaction();
            if (ConduitContext.Tags.Any()) return;
            foreach (var tag in DummyTags)
            {
                ConduitContext.Entry(tag).State = EntityState.Added;
            }

            if (ConduitContext.Users.Any()) return;
            foreach (var user in DummyUsers)
            {
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