using Microsoft.EntityFrameworkCore;
using Realworld.Api.Models;

namespace Realworld.Api.Data
{
    public class ArticleFavoriteRepository: IArticleFavoriteRepository {
        private readonly ConduitContext _context;
        public ArticleFavoriteRepository(ConduitContext context) { 
            _context = context;
        }

        public async Task<ArticleFavoriteLink?> GetArticleFavoriteAsync(string username, Guid articleId) {
            return await _context.ArticleFavoriteLinks.AsNoTracking().FirstOrDefaultAsync(afl => afl.ArticleId == articleId && afl.Username == username);
            //added AsNoTracking to avoid EF Core tracking:
        }
        public void AddArticleFavorite(ArticleFavoriteLink articleFavoriteLink) {
            _context.ArticleFavoriteLinks.Add(articleFavoriteLink);        
        }
        public void RemoveArticleFavorite(ArticleFavoriteLink articleFavoriteLink) {
            _context.ArticleFavoriteLinks.Remove(articleFavoriteLink);
        }
    }
}