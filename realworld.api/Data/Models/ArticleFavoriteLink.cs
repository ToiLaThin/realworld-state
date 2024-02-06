namespace Realworld.Api.Models
{
    //join table model
    public class ArticleFavoriteLink
    {
        public Guid ArticleId { get; set; }
        public Article Article { get; set; }
        public string Username { get; set; }
        public User User { get; set; }

        public ArticleFavoriteLink() {}

        public ArticleFavoriteLink(Guid articleId, string username)
        {
            ArticleId = articleId;
            Username = username;
        }
    }
}