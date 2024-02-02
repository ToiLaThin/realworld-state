using Realworld.Api.Models;

namespace Realworld.Api.Data
{
    public interface ICommentRepository {
        public void AddArticleComment(Comment comment);
        public void RemoveArticleComment(Comment comment);
        public Task<List<Comment>>GetCommentsBySlugAsync(string slug, string? username);
    }
}