namespace Realworld.Api.Models
{
    //join table model
    public class ArticleFavoriteLink
    {
        public Guid ArticleId { get; set; }
        public Article Article { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public ArticleFavoriteLink() {}
    }
}