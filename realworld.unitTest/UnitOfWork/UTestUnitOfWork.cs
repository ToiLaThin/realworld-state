using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Realworld.Api.Models;

namespace Realworld.UnitTest.UnitOfWork {
    public class UTestUnitOfWork: BaseUTestUnitOfWork, IDisposable {

        protected Realworld.Api.Data.UnitOfWork UnitOfWork { get; init; }

        public UTestUnitOfWork(FixtureUTestUnitOfWork fixture) : base(fixture) { 
            base.SeedDb();
            UnitOfWork = new Realworld.Api.Data.UnitOfWork(ConduitContext);
        }

        [Fact]
        [Trait("Category", "UnitTestUnitOfWork")]
        public async Task WhenAddNewArticleWithoutCommiting_ReturnArticleNotAddedResult() {
            //Arrange
            var transaction = await UnitOfWork.BeginTransactionAsync();
            var newArticle = new Article(title: "New Article", body: "New Article Description", description: "New Article Body") {
                Id = Guid.Parse("193a1623-47af-222e-9288-7b57a7c3a222"),
                Author = base.DummyUsers.First(),
                Tags = base.DummyTags.Skip(1).Take(2).ToList()
            };

            //Act 1
            UnitOfWork.ArticleRepository.AddArticle(newArticle);

            //Assert 1
            var expectedArticles = base.DummyArticles.ToList();
            var actualArticles = await ConduitContext.Articles.ToListAsync();
            actualArticles.Should().BeEquivalentTo(expectedArticles);
            actualArticles.Should().BeAssignableTo<IEnumerable<Article>>();

            //Act 2
            await UnitOfWork.CommitTransactionAsync(transaction);

            //Assert 2
            expectedArticles = base.DummyArticles.Concat(new List<Article>() { newArticle }).ToList();
            actualArticles = await ConduitContext.Articles.ToListAsync();
            actualArticles.Should().BeEquivalentTo(expectedArticles);
            actualArticles.Should().BeAssignableTo<IEnumerable<Article>>();

            //Clean Up
            ConduitContext.Articles.Entry(newArticle).State = EntityState.Deleted;
            await ConduitContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            ConduitContext.Users.RemoveRange(ConduitContext.Users.ToList());
            ConduitContext.Articles.RemoveRange(ConduitContext.Articles.ToList());
            ConduitContext.Tags.RemoveRange(ConduitContext.Tags.ToList());
            ConduitContext.SaveChanges();
        }
    }
}