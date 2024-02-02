namespace Realworld.Api.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public string Body { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string Username { get; set; }

        public User Author { get; set; } = null!;

        //foreign key of article
        public Guid ArticleId { get; set; }

        public Article Article { get; set; } = null!;

        public Comment(string body, string username, Guid articleId) {
            Body = body;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            Username = username;
            ArticleId = articleId;
        }
        
                
    }
}