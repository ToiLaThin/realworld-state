using Realworld.Api.Models;
using Realworld.Api.Dto;
using Realworld.Api.Data;

namespace Realworld.Api.Mapping {
    public static class ArticleMapper {
        public static ArticleSingleResponseDto MapArticleToArticleSingleResponseDto(Article article, bool isFollowing) {
            return new ArticleSingleResponseDto(
                article.Slug,
                article.Title,
                article.Description,
                article.Body,
                article.Tags.Select(t => t.Id).ToList(),
                article.CreatedAt,
                article.UpdatedAt,
                article.Favorited,
                article.FavoritesCount,
                new ProfileResponseDto(
                    article.Author.Username,
                    article.Author.Bio,
                    article.Author.Image,
                    isFollowing //this is calc in service then pass in here
                )    
            );
        }
    }
}