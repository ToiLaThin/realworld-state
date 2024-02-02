using Microsoft.EntityFrameworkCore;
using Realworld.Api.Models;

namespace Realworld.Api.Data
{
  public class ArticleRepository : IArticleRepository
  {
    private readonly ConduitContext _context;
    public ArticleRepository(ConduitContext context)
    {
      _context = context;
    }

    public void AddArticle(Article article)
    {
      _context.Articles.Add(article);
    }

    public void DeleteArticle(Article article)
    {
      _context.Articles.Remove(article);
    }

    public async Task<Article?> GetArticleBySlugAsync(string slug, bool asNoTracking)
    {
      //ThenInclude is from authen we continue include
      var query = _context.Articles.Include(a => a.Author).Include(a => a.Tags).AsQueryable();
      if (asNoTracking) {
        query = query.AsNoTracking();
      }

      var article = await query.FirstOrDefaultAsync(a => a.Slug == slug);
      if (article == null) {
        return null;
      }
      //calc
      article.FavoritesCount = await _context.ArticleFavoriteLinks.CountAsync(afl => afl.ArticleId == article.Id);
      article.Favorited = article.FavoritesCount > 0;
      return article;
    }

    public async Task<ArticlesResponseDto> GetArticlesAsync(ArticlesQuery articlesQuery, string? username, bool isFeed)
    {
      var query = _context.Articles.AsQueryable();

      if (!string.IsNullOrWhiteSpace(articlesQuery.Tag)) {
        query = query.Where(a => a.Tags.Any(t => t.Id == articlesQuery.Tag));
      }

      if (!string.IsNullOrWhiteSpace(articlesQuery.Author)) {
        query = query.Where(a => a.Author.Username == articlesQuery.Author);
      }

      //where is the favorited
      if (username is not null) {
        query.Include(x => x.Author).ThenInclude(x => x.Followers.Where(fu => fu.FollowerName == username));
      }

      //list article where author of that article is followed the current user
      if (isFeed) {
        query = query.Where(a => a.Author.Followers.Any(f => f.FollowerName == username));
      }

      var pagedQuery = query.Skip(articlesQuery.Offset)
                      .Take(articlesQuery.Limit)
                      .Include(a => a.Tags)
                      .Include(a => a.Author);
      var totalArticle = await query.CountAsync();
      return new ArticlesResponseDto(await pagedQuery.ToListAsync(), totalArticle);
    }
  }
}