using Realworld.Api.Extension;

namespace Realworld.Api.Models
{
    public class Article
    {
        public Guid Id { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }

        public string Body { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public User Author { get; set; } = null!;

        public ICollection<Tag> Tags { get; set; } = new List<Tag>();

        public ICollection<Comment> ArticleComments { get; set; } = new List<Comment>();

        //this article is favorited by these users
        public ICollection<ArticleFavoriteLink> UserFavoritesArticleMappings { get; set; } = new List<ArticleFavoriteLink>();

        //computed properties
        public bool Favorited { get; set; }

        public int FavoritesCount { get; set; } = 0;
        public Article(string title, string description, string body) {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            Body = body;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            Slug = title.GenerateSlug();
        }
                
    }
}