namespace Realworld.Api.Models
{
    public class Tag
    {
        public string Id { get; set; } //tag name is the id

        public ICollection<Article> Articles { get; set; }

        public Tag() {}

    }
}