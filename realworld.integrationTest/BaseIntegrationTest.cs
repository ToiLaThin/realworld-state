using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Realworld.Api.Data;
using Realworld.Api.Models;
using Realworld.Api.Utils;
using Realworld.IntegrationTest.Utils;

namespace Realworld.IntegrationTest {
    public class BaseIntegrationTest: IClassFixture<ConduitWebAppFactory>, IDisposable {
        protected ConduitContext ConduitContext { get; private set; }

        private DummyDataProvider _dummyDataProvider;
        protected ConduitWebAppFactory ConduitWebAppFactory { get; private set; }

        protected IEnumerable<User> DummyUsers { get; init; }

        protected IEnumerable<Tag> DummyTags { get; init; }

        protected IEnumerable<Article> DummyArticles { get; init; }

        protected IEnumerable<Comment> DummyComments { get; init; }
        public BaseIntegrationTest(ConduitWebAppFactory webAppFactory) {
            ConduitWebAppFactory = webAppFactory;
            var scope = webAppFactory.Services.CreateScope();
            ConduitContext = scope.ServiceProvider.GetRequiredService<ConduitContext>();

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
        }

        protected virtual void SeedDb() {

        }

        public void Dispose()
        {
            var transaction = ConduitContext.Database.BeginTransaction();
            ConduitContext.Users.RemoveRange(ConduitContext.Users.ToList());
            ConduitContext.Articles.RemoveRange(ConduitContext.Articles.ToList());
            ConduitContext.Tags.RemoveRange(ConduitContext.Tags.ToList());
            ConduitContext.SaveChanges();
            transaction.Commit();
            ConduitContext.Dispose();
        }
  }
}