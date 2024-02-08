using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Realworld.Api.Dto;
using Realworld.Api.Mapping;
using Realworld.Api.Models;
using Realworld.Api.Services;
using Realworld.Api.Utils.Auth;

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
        //no need auth, but we can still get current user name from httpContext
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policy.OptionalAuthenticated)]
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policy.OptionalAuthenticated)]    
        public async Task<ArticlesMultipleEnvelope<ArticleSingleResponseDto>> ListArticlesAsync([FromQuery] ArticlesQueryDto articlesQuery) { //model from query instead of from body
            string currentUsername = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value; //can be null or not
            var articlesWithTotalCount = await _articleService.ListArticlesAsync(articlesQuery, false);
            var articleSingleRespList = articlesWithTotalCount.Articles.Select(a => {
                bool isCurrUserFollowingArticleAuthor = currentUsername != null ?
                        a.Author.Followers.Any(f => f.FollowerName == currentUsername) :
                        false; //if not login, then following = false
                return ArticleMapper.MapArticleToArticleSingleResponseDto(a, isCurrUserFollowingArticleAuthor);
            }).ToList();
            return new ArticlesMultipleEnvelope<ArticleSingleResponseDto>(articleSingleRespList, articlesWithTotalCount.ArticlesCount);
        }

        [HttpGet("api/articles/feed")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ArticlesMultipleEnvelope<ArticleSingleResponseDto>> FeedArticlesAsync([FromQuery] ArticlesFeedQueryDto articlesFeedQuery) {
            string currentUsername = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ArticlesQueryDto articlesQuery = new ArticlesQueryDto(null, null, null, articlesFeedQuery.Limit, articlesFeedQuery.Offset);
            var articlesWithTotalCount = await _articleService.ListArticlesAsync(articlesQuery, isFeed: true);
            var articleSingleRespList = articlesWithTotalCount.Articles.Select(a => {
                bool isCurrUserFollowingArticleAuthor = a.Author.Followers.Any(f => f.FollowerName == currentUsername);
                return ArticleMapper.MapArticleToArticleSingleResponseDto(a, isCurrUserFollowingArticleAuthor);
            }).ToList();
            return new ArticlesMultipleEnvelope<ArticleSingleResponseDto>(articleSingleRespList, articlesWithTotalCount.ArticlesCount);
        }
    }
}