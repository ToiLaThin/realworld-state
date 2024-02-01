namespace Realworld.Api.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public string Body { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public User Author { get; set; }

        //foreign key of article
        public Guid ArticleId { get; set; }

        public Article Article { get; set; }

        public Comment() {}
        
                
    }
}