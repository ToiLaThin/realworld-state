using Realworld.Api.Models;

namespace Realworld.Api.Data
{
    public interface IArticleFavoriteRepository {
        public Task<ArticleFavoriteLink?> GetArticleFavoriteAsync(string username, Guid articleId);
        public void AddArticleFavorite(ArticleFavoriteLink articleFavoriteLink);
        public void RemoveArticleFavorite(ArticleFavoriteLink articleFavoriteLink);
    }
}