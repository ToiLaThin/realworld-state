using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Realworld.Api.Models;

namespace Realworld.UnitTest.Repository
{
    public class UTestCommentRepository: BaseUTestRepository {
        protected Realworld.Api.Data.CommentRepository CommentRepository { get; init; }
        public UTestCommentRepository(FixtureUTestRepository fixture) : base(fixture)
        {
            base.SeedDb();
            CommentRepository = new Realworld.Api.Data.CommentRepository(base.ConduitContext);
        }
        
        [Fact]
        public async Task WhenGetCommentsByExistingArticleSlugAndNoFollower_ReturnValidCommentWithNoFollowerOfThatUsername()
        {
            //Arrange
            var existingArticle = base.DummyArticles.First();
            var existingArticleSlug = existingArticle.Slug;
            // var firstUsernameCommentExistingArticle = base.DummyComments.Where(c => c.ArticleId == existingArticle.Id)
            //                                                             .Select(c => c.Username)
            //                                                             .First();
            var expectedComments = base.ConduitContext.Comments.Where(c => c.Article.Slug == existingArticleSlug)
                                                               .ToList();


            //Act
            var actualComments = await CommentRepository.GetCommentsBySlugAsync(existingArticleSlug, null);
            
            //Assert
            actualComments.Should().BeEquivalentTo(expectedComments);
            actualComments.First().Should().Match<Comment>(c => c.Author != null);
            actualComments.First().Should().Match<Comment>(c => c.Author.Followers == null || c.Author.Followers.Count == 0);
            actualComments.Should().BeAssignableTo(typeof(IEnumerable<Comment>));
        }

        [Fact]
        public async Task WhenAddCommentToArticleNoneExisting_ReturnArticleWithCommentAdded()
        {
            //Arrange
            var existingThirdArticle = base.DummyArticles.Skip(2).First();
            var existingFirstUser = base.DummyUsers.First();

            //Act
            var trans = await ConduitContext.Database.BeginTransactionAsync();
            var addedComment = new Comment(body: "new comment", username: existingFirstUser.Username,articleId: existingThirdArticle.Id);
            CommentRepository.AddArticleComment(addedComment);
            await ConduitContext.SaveChangesAsync();
            await trans.CommitAsync();
            
            //Assert
            var expectedComments = base.ConduitContext.Comments.Where(c => c.ArticleId == existingThirdArticle.Id && c.Username == existingFirstUser.Username)
                                                               .ToList();
            expectedComments.Count().Should().Be(1);
            expectedComments.First().Should().BeEquivalentTo(addedComment);
            expectedComments.Should().BeAssignableTo(typeof(IEnumerable<Comment>));
            //when running tests async, the added must be deleted back to avoid data inconsistency with other tests
            ConduitContext.Comments.Remove(addedComment);
            await ConduitContext.SaveChangesAsync();            
        }

    }
}