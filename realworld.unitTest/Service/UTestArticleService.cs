using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Realworld.Api.Data;
using Realworld.Api.Mapping;
using Realworld.Api.Models;
using Realworld.Api.Services;
using Realworld.Api.Utils;
using Realworld.Api.Utils.ExceptionHandling;
using Realworld.UnitTest.Utils;

namespace Realworld.UnitTest.Service {
    public class UTestArticleService {
        private DummyDataProvider _dummyDataProvider;

        public UTestArticleService() {
            var jwtOptions = Options.Create(new JwtOptions()
            {
                Audience = "https://localhost:5001",
                Issuer = "https://localhost:5001",
                Secret = "supersecret  have to have the required length"
            });
            var jwtTokenGenerator = new JwtTokenGenerator(jwtOptions);
            _dummyDataProvider = new DummyDataProvider(jwtTokenGenerator);
        }
        [Fact]
        public async Task GivenSlugOfExistingArticleAndUnauthenticatedUser_WhenGetArticleBySlug_ReturnValidArticle() {
            //Arrange 
            var expectedArticle = _dummyDataProvider.GetDummyArticles().First();
            var expectedArticleSlug = expectedArticle.Slug;
            var returnComments = _dummyDataProvider.GetDummyComments().Where(c => c.ArticleId == expectedArticle.Id).ToList();

            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            Mock<ICurrentUsernameAccessor> mockCurrentUsernameAccessor = new Mock<ICurrentUsernameAccessor>();
            mockCurrentUsernameAccessor.Setup(cua => cua.GetCurrentUsername()).Returns<string?>(null);

            //asNoTracking is true because in the articleService, we passed in true, if use false, this method will return null due to wrong setup
            //username is null , because we setup the mockCurrentUsernameAccessor to return null
            mockUnitOfWork.Setup(uow => uow.ArticleRepository.GetArticleBySlugAsync(It.IsAny<string>(), true, null)) 
            .ReturnsAsync(expectedArticle);

            mockUnitOfWork.Setup(uow => uow.CommentRepository.GetCommentsBySlugAsync(It.IsAny<string>(), null))
            .ReturnsAsync(returnComments);

            var sut = new ArticleService(mockUnitOfWork.Object, mockCurrentUsernameAccessor.Object);

            //Act
            var actualArticle = await sut.GetArticleBySlugAsync(expectedArticleSlug);

            //Assert
            actualArticle.Should().NotBeNull();
            actualArticle.Should().BeAssignableTo<ArticleSingleResponseDto>();
            actualArticle.Should().BeEquivalentTo(ArticleMapper.MapArticleToArticleSingleResponseDto(expectedArticle, false));
            //due to anonymous user, the following should be false
            actualArticle.Should().Match<ArticleSingleResponseDto>(a => a.Author.Following == false);
        }

        [Fact]
        public async Task GivenSlugOfExistingArticleAndAuthenticatedUser_WhenGetArticleBySlug_ReturnValidArticle() {
            //Arrange 
            var expectedArticle = _dummyDataProvider.GetDummyArticles().First();
            string firstUsername = _dummyDataProvider.GetDummyUsers().First().Username;
            var expectedArticleSlug = expectedArticle.Slug;
            var returnComments = _dummyDataProvider.GetDummyComments().Where(c => c.ArticleId == expectedArticle.Id).ToList();

            //the current user is following the author of the article (which coincides with the first user in the dummy data)
            var userLinkToAdd = new UserLink(expectedArticle.Author.Username, firstUsername);
            expectedArticle.Author.Followers.Add(userLinkToAdd);

            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            Mock<ICurrentUsernameAccessor> mockCurrentUsernameAccessor = new Mock<ICurrentUsernameAccessor>();
            mockCurrentUsernameAccessor.Setup(cua => cua.GetCurrentUsername()).Returns(firstUsername);

            //asNoTracking is true because in the articleService, we passed in true, if use false, this method will return null due to wrong setup
            //username is null , because we setup the mockCurrentUsernameAccessor to return null
            mockUnitOfWork.Setup(uow => uow.ArticleRepository.GetArticleBySlugAsync(It.IsAny<string>(), true, firstUsername)) 
            .ReturnsAsync(expectedArticle); //the setup user coincides with the author of the article, prepared data is not favorited by the user (expectedArticle)

            mockUnitOfWork.Setup(uow => uow.CommentRepository.GetCommentsBySlugAsync(It.IsAny<string>(), null))
            .ReturnsAsync(returnComments);

            var sut = new ArticleService(mockUnitOfWork.Object, mockCurrentUsernameAccessor.Object);

            //Act
            var actualArticle = await sut.GetArticleBySlugAsync(expectedArticleSlug);

            //Assert
            actualArticle.Should().NotBeNull();
            actualArticle.Should().BeAssignableTo<ArticleSingleResponseDto>();
            actualArticle.Should().Match<ArticleSingleResponseDto>(a => a.Author.Following == true);
            actualArticle.Should().BeEquivalentTo(ArticleMapper.MapArticleToArticleSingleResponseDto(expectedArticle, true));
        }

        [Fact]
        public async Task GivenSlugOfNoneExisting_WhenGetArticleBySlug_ThrowConduitException() {
            //Arrange 
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            Mock<ICurrentUsernameAccessor> mockCurrentUsernameAccessor = new Mock<ICurrentUsernameAccessor>(); //no setup, so it will return null

            Article? nullArticle = null;
            mockUnitOfWork.Setup(uow => uow.ArticleRepository.GetArticleBySlugAsync(It.IsAny<string>(), true, null)) 
            .ReturnsAsync(nullArticle); 

            var sut = new ArticleService(mockUnitOfWork.Object, mockCurrentUsernameAccessor.Object);

            //Act
            //Assert
            Assert.ThrowsAsync<ConduitException>(() => sut.GetArticleBySlugAsync("none-existing-slug-or-any"));
        }
    }
}