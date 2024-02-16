using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Realworld.Api.Controllers;
using Realworld.Api.Mapping;
using Realworld.Api.Services;
using Realworld.Api.Utils;
using Realworld.UnitTest.Utils;

namespace Realworld.UnitTest.Controller {
    public class UTestArticleController {
        protected DummyDataProvider _dummyDataProvider;
        public UTestArticleController() {
            var jwtOptions = Options.Create(new JwtOptions() {
                Audience = "https://localhost:5001",
                Issuer = "https://localhost:5001",
                Secret = "supersecret  have to have the required length"
            });
            var jwtTokenGenerator = new JwtTokenGenerator(jwtOptions);
            _dummyDataProvider = new DummyDataProvider(jwtTokenGenerator);
        }

        [Fact] 
        public async Task WhenGetArticleBySlug_ReturnArticleEnvelope() {
            //Arrange
            var mockArticleService = new Mock<IArticleService>();
            var expectedArticle = _dummyDataProvider.GetDummyArticles().First();
            var expectedArticleSlug = expectedArticle.Slug;
            var expectedArticleSingleResponseDto = ArticleMapper.MapArticleToArticleSingleResponseDto(expectedArticle, false);
            var expectedArticleSingleEnvelope = new ArticleSingleEnvelope<ArticleSingleResponseDto>(expectedArticleSingleResponseDto);

            mockArticleService.Setup(s => s.GetArticleBySlugAsync(It.IsAny<string>())).ReturnsAsync(expectedArticleSingleResponseDto);
            var sut = new ArticleController(mockArticleService.Object);

            //Act
            var actualArticleSingleEnvelope = await sut.GetArticleBySlugAsync(expectedArticleSlug);

            //Assert
            actualArticleSingleEnvelope.Should().NotBeNull();
            actualArticleSingleEnvelope.Should().BeAssignableTo<ArticleSingleEnvelope<ArticleSingleResponseDto>>();
            actualArticleSingleEnvelope.Should().BeEquivalentTo(expectedArticleSingleEnvelope);
        }
    }
}