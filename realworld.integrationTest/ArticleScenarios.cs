using System.Web;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Realworld.Api.Controllers;
using Realworld.Api.Mapping;
using Realworld.Api.Models;
using Realworld.IntegrationTest.Utils;

namespace Realworld.IntegrationTest {
    public class ArticleScenario: BaseIntegrationTest {
        public ArticleScenario(ConduitWebAppFactory conduitWebAppFactory): base(conduitWebAppFactory) {
            this.SeedDb();
        }

        protected override void SeedDb()
        {
            //base.SeedDb();
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

        [Fact]
        public async Task GivenAnAnonymousNormalQuery_WhenListArticlesAsync_ReturnValidResponse() {
            //Assert
            var client = ConduitWebAppFactory.CreateClient();
            var queryDto = new ArticlesQueryDto(
                Tag: null,
                Author: null,
                Favorited: null
            );
            string baseUrl = IntegrationTestEndpoint.Article.ListArticles;
            var query = HttpUtility.ParseQueryString(String.Empty);
            query["limit"] = queryDto.Limit.ToString();
            query["offset"] = queryDto.Offset.ToString();
            string finalUrlQueryString = baseUrl + "?" + query.ToString();

            var expectedArticleSingleRespDtoList = base.DummyArticles.Select(
                s => {
                    s.Tags = new List<Tag>(); //get does not get tags, also we must set createdAt, updatedAt to fixed
                    return ArticleMapper.MapArticleToArticleSingleResponseDto(s, false);
                }
            ).ToList();
            var expectedTotalArticleCount = expectedArticleSingleRespDtoList.Count();
            var expectedArticleMultipleEnvelope = new ArticlesMultipleEnvelope<ArticleSingleResponseDto>(expectedArticleSingleRespDtoList, expectedTotalArticleCount);

            //Act
            var response = await client.GetAsync(finalUrlQueryString);
            var actualArticlesMultipleEnvelope = JsonConvert.DeserializeObject<ArticlesMultipleEnvelope<ArticleSingleResponseDto>>(
                await response.Content.ReadAsStringAsync()
            );

            //Arrange
            Assert.NotNull(response);
            response.StatusCode.Should().BeOneOf(System.Net.HttpStatusCode.OK);
            actualArticlesMultipleEnvelope.Should().BeAssignableTo(
                typeof(ArticlesMultipleEnvelope<ArticleSingleResponseDto>)
            );            
            actualArticlesMultipleEnvelope.Should().BeEquivalentTo(expectedArticleMultipleEnvelope);            
        }

  }
}