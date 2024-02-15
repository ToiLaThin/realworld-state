using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Realworld.Api.Models;

namespace Realworld.UnitTest.Repository
{
    public class UTestArticleFavorite : BaseUTestRepository
    {
        protected Realworld.Api.Data.ArticleFavoriteRepository ArticleFavoriteRepository { get; init; }
        public UTestArticleFavorite(FixtureUTestRepository fixture) : base(fixture)
        {
            base.SeedDb();
            ArticleFavoriteRepository = new Realworld.Api.Data.ArticleFavoriteRepository(base.ConduitContext);
        }

        [Fact]
        public async Task GivenUserNotFavoriteArticle_WhenGetArticleFavorite_ReturnNullResult()
        {
            //Arrange
            var existingThirdArticle = base.DummyArticles.Skip(2).First();
            var existingFirstUser = base.DummyUsers.First();

            //Act
            var actualArticleFavorite = await ArticleFavoriteRepository.GetArticleFavoriteAsync(existingFirstUser.Username, existingThirdArticle.Id);

            //Assert
            actualArticleFavorite.Should().BeNull();
        }

        [Fact]
        public async Task GivenUserNotFavoriteArticle_WhenAddArticleFavoriteForUser_ReturnFavoriteArticleResult()
        {
            //Arrange
            var existingThirdArticle = base.DummyArticles.Skip(2).First();
            var existingFirstUser = base.DummyUsers.First();

            //Act
            var toAddArticleFavorite = new Realworld.Api.Models.ArticleFavoriteLink(username: existingFirstUser.Username, articleId: existingThirdArticle.Id);
            ArticleFavoriteRepository.AddArticleFavorite(toAddArticleFavorite);
            await ConduitContext.SaveChangesAsync();

            //Assert
            var actualArticleFavorite = await ConduitContext.ArticleFavoriteLinks.FirstOrDefaultAsync(afl => afl.ArticleId == existingThirdArticle.Id && afl.Username == existingFirstUser.Username); //no use GetArticleFavoriteAsync because it's have asNoTracking()
            actualArticleFavorite.Should().NotBeNull();
            actualArticleFavorite.Should().Match<ArticleFavoriteLink>(afl => afl.ArticleId.Equals(existingThirdArticle.Id) && afl.Username.Equals(existingFirstUser.Username));
            actualArticleFavorite.Should().Match<ArticleFavoriteLink>(afl => afl.Article.Equals(existingThirdArticle) && afl.User.Equals(existingFirstUser));

            //Clean Up
            ConduitContext.ArticleFavoriteLinks.Entry(actualArticleFavorite).State = EntityState.Deleted;
            await ConduitContext.SaveChangesAsync();
        }

        [Fact]
        public async Task GivenUserFavoritedArticle_WhenRemoveArticleFavoriteForUser_ReturnNotFavoriteArticleResult()
        {
            //Arrange
            var existingThirdArticle = base.DummyArticles.Skip(2).First();
            var existingFirstUser = base.DummyUsers.First();
            var toAddArticleFavorite = new Realworld.Api.Models.ArticleFavoriteLink(username: existingFirstUser.Username, articleId: existingThirdArticle.Id);
            ConduitContext.ArticleFavoriteLinks.Entry(toAddArticleFavorite).State = EntityState.Added;
            ConduitContext.SaveChanges();

            //Act
            ArticleFavoriteRepository.RemoveArticleFavorite(toAddArticleFavorite);
            await ConduitContext.SaveChangesAsync();

            //Assert
            var actualArticleFavorite = await ConduitContext.ArticleFavoriteLinks.FirstOrDefaultAsync(afl => afl.ArticleId == existingThirdArticle.Id && afl.Username == existingFirstUser.Username); //no use GetArticleFavoriteAsync because it's have asNoTracking()
            actualArticleFavorite.Should().BeNull();
        }
    }
}