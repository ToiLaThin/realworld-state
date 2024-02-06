using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Realworld.Api.Dto;
using Realworld.Api.Models;
using Realworld.Api.Services;

namespace Realworld.Api.Controllers {

    public record ArticleSingleEnvelope<T>(T Article);

    public record ArticlesMultipleEnvelope<T>(List<T> Articles, int ArticlesCount);

    [ApiController]
    public class ArticleController: ControllerBase {
        private readonly IArticleService _articleService;
        public ArticleController(IArticleService articleService) {
            _articleService = articleService;
        }

        [HttpGet("api/articles/{slug}")]
        //no need auth, but we can still get current user name from httpContext (must put on [Authorize schme = JwtBearerDefaults.AuthenticationScheme])
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [AllowAnonymous]
        public async Task<ArticleSingleEnvelope<ArticleSingleResponseDto>> GetArticleBySlugAsync(string slug) {
            //var user = User.Claims;
            var article = await _articleService.GetArticleBySlugAsync(slug);
            return new ArticleSingleEnvelope<ArticleSingleResponseDto>(article);
        }

        [HttpPost("api/articles")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ArticleSingleEnvelope<ArticleSingleResponseDto>> CreateArticleAsync(CreateArticleRequestDto createArticleReq) {
            var article = await _articleService.CreateArticleAsync(createArticleReq);
            return new ArticleSingleEnvelope<ArticleSingleResponseDto>(article);
        }

        [HttpPut("api/articles/{slug}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ArticleSingleEnvelope<ArticleSingleResponseDto>> UpdateArticleAsync(string slug,[FromBody] UpdateArticleRequestDto updateArticleReq) {
            var updatedArticleResp = await _articleService.UpdateArticleAsync(slug, updateArticleReq);
            return new ArticleSingleEnvelope<ArticleSingleResponseDto>(updatedArticleResp);
        }

        [HttpDelete("api/articles/{slug}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ArticleSingleEnvelope<ArticleSingleResponseDto>> DeleteArticleAsync(string slug) {
            var deletedArticleResp = await _articleService.DeleteArticleAsync(slug);
            return new ArticleSingleEnvelope<ArticleSingleResponseDto>(deletedArticleResp);
        }

        [HttpPost("api/articles/{slug}/favorite")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ArticleSingleEnvelope<ArticleSingleResponseDto>> FavoriteArticleAsync(string slug) {
            var favoritedArticleResp = await _articleService.FavoriteArticleAsync(slug);
            return new ArticleSingleEnvelope<ArticleSingleResponseDto>(favoritedArticleResp);
        }

        [HttpDelete("api/articles/{slug}/favorite")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ArticleSingleEnvelope<ArticleSingleResponseDto>> UnfavoriteArticleAsync(string slug) {
            var unfavoritedArticleResp = await _articleService.UnfavoriteArticleAsync(slug);
            return new ArticleSingleEnvelope<ArticleSingleResponseDto>(unfavoritedArticleResp);
        }

        [HttpGet("api/articles")]        
        public async Task<ArticlesMultipleEnvelope<ArticleSingleResponseDto>> ListArticlesAsync([FromQuery] ArticlesQueryDto articlesQuery) { //model from query instead of from body
            var articles = await _articleService.ListArticlesAsync(articlesQuery, false);
            var articleSingleRespList = articles.Articles.Select(a => new ArticleSingleResponseDto(
            a.Slug,
            a.Title,
            a.Description,
            a.Body,
            a.Tags.Select(t => t.Id).ToList(),
            a.CreatedAt,
            a.UpdatedAt,
            a.Favorited,
            a.FavoritesCount,
            new ProfileResponseDto(
                a.Author.Username,
                a.Author.Bio,
                a.Author.Image,
                //a.Author.Followers.Any(f => f.FollowerName == currentUsername) TODO: implement this
                false
            )
            )).ToList();
            return new ArticlesMultipleEnvelope<ArticleSingleResponseDto>(articleSingleRespList, articles.ArticlesCount);
        }

        [HttpGet("api/articles/feed")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ArticlesMultipleEnvelope<ArticleSingleResponseDto>> FeedArticlesAsync([FromQuery] ArticlesFeedQueryDto articlesFeedQuery) {
            ArticlesQueryDto articlesQuery = new ArticlesQueryDto(null, null, null, articlesFeedQuery.Limit, articlesFeedQuery.Offset);
            var articles = await _articleService.ListArticlesAsync(articlesQuery, isFeed: true);
            var articleSingleRespList = articles.Articles.Select(a => new ArticleSingleResponseDto(
            a.Slug,
            a.Title,
            a.Description,
            a.Body,
            a.Tags.Select(t => t.Id).ToList(),
            a.CreatedAt,
            a.UpdatedAt,
            a.Favorited,
            a.FavoritesCount,
            new ProfileResponseDto(
                a.Author.Username,
                a.Author.Bio,
                a.Author.Image,
                //a.Author.Followers.Any(f => f.FollowerName == currentUsername) TODO: implement this
                true
            )
            )).ToList();
            return new ArticlesMultipleEnvelope<ArticleSingleResponseDto>(articleSingleRespList, articles.ArticlesCount);
        }
    }
}