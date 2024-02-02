using Microsoft.EntityFrameworkCore;
using Realworld.Api.Models;

namespace Realworld.Api.Data
{
  public class CommentRepository : ICommentRepository
  {
    private readonly ConduitContext _context;
    public CommentRepository(ConduitContext context) { 
        _context = context;
    }
    public void AddArticleComment(Comment comment)
    {
        _context.Comments.Add(comment);
    }

    public async Task<List<Comment>> GetCommentsBySlugAsync(string slug, string? username)
    {
        return await _context.Comments.Where(c => c.Article.Slug == slug)
                                      .Include(c => c.Author)
                                      .ThenInclude(a => a.Followers.Where(f => f.Follower.Username == username))
                                      .ToListAsync();
    }

    public void RemoveArticleComment(Comment comment)
    {
        _context.Comments.Remove(comment);
    }
  }
}