using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Realworld.Api.Models;
using Realworld.UnitTest.Utils;

namespace Realworld.UnitTest.Repository
{
  public class UTestArticleRepository : BaseUTestRepository
  {
    protected Realworld.Api.Data.ArticleRepository ArticleRepository { get; set; }
    public UTestArticleRepository(FixtureUTestRepository fixture) : base(fixture)
    {
      base.SeedDb();
      ArticleRepository = new Realworld.Api.Data.ArticleRepository(base.ConduitContext);
    }

    [Fact]
    [Trait("Category", "UnitTestRepository")]
    public async Task GivenSlugOfExistingArticle_WhenGetArticleBySlug_ReturnValidArticle()
    {
      //Arange
      var expectedArticle = ConduitContext.Articles.First();
      var expectedArticleSlug = expectedArticle.Slug;

      //Act
      var actualArticle = await ArticleRepository.GetArticleBySlugAsync(expectedArticleSlug, false);

      //Assert
      actualArticle.Should().NotBeNull();
      actualArticle.Should().BeAssignableTo<Article>();
      actualArticle.Should().BeEquivalentTo(expectedArticle);
    }

    [Fact]
    [Trait("Category", "UnitTestRepository")]
    public async Task WhenAddNewArticle_ReturnArticleAdded()
    {

      //Arrange
      var newArticle = new Article(title: "New Article", body: "New Article Description", description: "New Article Body")
      {
        Id = Guid.Parse("193a1623-47af-222e-9288-7b57a7c3a222"),
        Author = base.DummyUsers.First(),
        Tags = base.DummyTags.Skip(1).Take(2).ToList()
      };

      //Act
      ArticleRepository.AddArticle(newArticle);
      await ConduitContext.SaveChangesAsync();


      //Assert
      var actualArticle = await ConduitContext.Articles.FirstOrDefaultAsync(a => a.Title == newArticle.Title);
      actualArticle.Should().NotBeNull();
      actualArticle.Should().BeEquivalentTo(newArticle);
      actualArticle.Should().BeAssignableTo<Article>();

      //Clean Up
      ConduitContext.Articles.Entry(actualArticle).State = EntityState.Deleted;
      await ConduitContext.SaveChangesAsync();
    }

    [Fact]
    [Trait("Category", "UnitTestRepository")]
    public async Task WhenDeleteArticle_ReturnArticleDeleted()
    {
      //Arrange
      var articleToBeDeleted = new Article(title: "New Article", body: "New Article Description", description: "New Article Body")
      {
        Id = Guid.Parse("193a1623-47af-222e-9288-7b57a7c3a222"),
        Author = base.DummyUsers.First(),
        Tags = base.DummyTags.Skip(1).Take(2).ToList()
      };
      ConduitContext.Articles.Entry(articleToBeDeleted).State = EntityState.Added;
      await ConduitContext.SaveChangesAsync();

      //Act
      ArticleRepository.DeleteArticle(articleToBeDeleted);
      await ConduitContext.SaveChangesAsync();

      //Assert
      var deletedArticle = await ConduitContext.Articles.FirstOrDefaultAsync(a => a.Id == articleToBeDeleted.Id);
      deletedArticle.Should().BeNull();
    }

    [Theory]
    [Trait("Category", "UnitTestRepository")]
    [InlineData(2, 1)]
    [InlineData(3, 0)]
    [InlineData(1, 2)]
    public async Task GivenQueryArticleByTagWithLimit_WhenGetArticles_ReturnValidArticles(int limit, int offset) {
      //Arrange
      string tagToQuery = base.DummyTags.Skip(1).Take(1).First().Id;
      ArticlesQueryDto query = new ArticlesQueryDto(Author: null, Favorited: null, Tag: tagToQuery, Limit: limit, Offset: offset);
      var expectedArticles = base.ConduitContext.Articles.Where(a => a.Tags.Any(t => t.Id == tagToQuery))
                                                         .Take(query.Limit)
                                                         .Skip(query.Offset)
                                                         .ToList();
      var expectedCount = expectedArticles.Count();

      //Act
      var articlesWithTotalCount = await ArticleRepository.GetArticlesAsync(query, null, false);
      var actualArticles = articlesWithTotalCount.Articles;
      var actualCount = articlesWithTotalCount.ArticlesCount;

      //Assert
      actualArticles.Should().BeEquivalentTo(expectedArticles);
      actualArticles.Should().BeAssignableTo(typeof(IEnumerable<Article>));
      actualArticles.Count().Should().Be(expectedArticles.Count());
      actualCount.Should().Be(expectedCount);
    }

    [Theory]
    [Trait("Category", "UnitTestRepository")]
    [InlineData(2, 1)]
    [InlineData(3, 0)]
    [InlineData(1, 2)]
    public async Task GivenQueryArticleByTagAndAuthorWithLimit_WhenGetArticles_ReturnValidArticles(int limit, int offset) {
      //Arrange
      string tagToQuery = base.DummyTags.Skip(1).Take(1).First().Id;
      string authorToQuery = base.DummyUsers.Skip(1).Take(1).First().Username;
      ArticlesQueryDto query = new ArticlesQueryDto(Author: null, Favorited: null, Tag: tagToQuery, Limit: limit, Offset: offset);
      var expectedArticles = base.ConduitContext.Articles.Where(a => a.Tags.Any(t => t.Id.Equals(tagToQuery)))
                                                         .Where(a => a.Author.Username.Equals(authorToQuery))
                                                         .Take(query.Limit)
                                                         .Skip(query.Offset)
                                                         .ToList();
      var expectedCount = expectedArticles.Count();

      //Act
      var articlesWithTotalCount = await ArticleRepository.GetArticlesAsync(query, null, false);
      var actualArticles = articlesWithTotalCount.Articles;
      var actualCount = articlesWithTotalCount.ArticlesCount;

      //Assert
      actualArticles.Should().BeEquivalentTo(expectedArticles);
      actualArticles.Should().BeAssignableTo(typeof(IEnumerable<Article>));
      actualArticles.Count().Should().Be(expectedArticles.Count());
      actualCount.Should().Be(expectedCount);
    }
  }
}