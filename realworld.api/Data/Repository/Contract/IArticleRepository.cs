using Realworld.Api.Models;

namespace Realworld.Api.Data
{
    public interface IArticleRepository {
        public Task<Article?> GetArticleBySlugAsync(string slug, bool asNoTracking);

        public Task<ArticlesResponseDto> GetArticlesAsync(ArticlesQuery articlesQuery, string? username, bool isFeed);

        public void AddArticle(Article article);
        
        public void DeleteArticle(Article article);
    }
}