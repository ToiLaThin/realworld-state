namespace Realworld.Api.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string Token { get; set; }

        public string Username { get; set; }

        public string Bio { get; set; }

        public string Image { get; set; }

        public string Password { get; set; }

        //join tables, navigation properties
        //1 to n mapping
        public ICollection<Comment> CommentsAboutArticle { get; set; }

        //n to n mapping
        public ICollection<ArticleFavoriteLink> ArticlesFavoritedByUserMapping { get; set; }

        public User() {}

        
                
    }
}