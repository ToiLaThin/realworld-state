using Npgsql.Replication;
using Realworld.Api.Extension;
using Realworld.Api.Models;
using Realworld.Api.Utils;
using Renci.SshNet;

namespace Realworld.UnitTest.Utils
{
    public class DummyDataProvider
    {
        private JwtTokenGenerator _jwtTokenGenerator;

        public DummyDataProvider(JwtTokenGenerator jwtTokenGenerator)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public IEnumerable<User> GetDummyUsers()
        {
            return new List<User>
            {
                new User() {
                    Username = "user1",
                    Email = "user1@gmail.com",
                    Password = "user1",
                    Token = this._jwtTokenGenerator.GenerateToken("user1"),
                    Bio = String.Empty,
                    Image = String.Empty,
                },
                new User() {
                    Username = "user2",
                    Email = "user2@gmail.com",
                    Password = "user2",
                    Token = this._jwtTokenGenerator.GenerateToken("user2"),
                    Bio = String.Empty,
                    Image = String.Empty,
                },
                new User() {
                    Username = "user3",
                    Email = "user3@gmail.com",
                    Password = "user3",
                    Token = this._jwtTokenGenerator.GenerateToken("user3"),
                    Bio = String.Empty,
                    Image = String.Empty,
                }
            };                    
        }

        public IEnumerable<Tag> GetDummyTags()
        {
            return new List<Tag>
            {
                new Tag(id: "tag1"),
                new Tag(id: "tag2"),
                new Tag(id: "tag3")
            };
        }

        public IEnumerable<Article> GetDummyArticles() {
            return new List<Article>() {
                new Article(title: "How to train your dragon", body: "It takes a Jacobian", description: "Ever wonder how?") {
                    Id = Guid.Parse("193a1623-47af-46ce-9288-7b57a7c3afa1"),
                    Author = this.GetDummyUsers().First(),
                    Tags = this.GetDummyTags().Take(2).ToList()
                },
                new Article(title: "Is coding still worth it", body: "Yes it is", description: "But you need to be patient") {
                    Id = Guid.Parse("8f1d9922-965b-4fb0-be28-2f4878666125"),
                    Author = this.GetDummyUsers().First(),
                    Tags = this.GetDummyTags().Take(3).ToList()
                },
                new Article(title: "How to train your dragon 2", body: "It takes a Jacobian 2", description: "Ever wonder how 2?") {
                    Id = Guid.Parse("0bc3c2b3-bf42-41c5-bc8b-a560f05a2f3d"),
                    Author = this.GetDummyUsers().First(),
                    Tags = this.GetDummyTags().Skip(1).Take(2).ToList()
                }
            };            
        }

        public IEnumerable<Comment> GetDummyComments() {
            var firstUser = this.GetDummyUsers().First();
            var firstArticle = this.GetDummyArticles().First();                        
            var secondUser = this.GetDummyUsers().Skip(1).First();
            var secondArticle = this.GetDummyArticles().Skip(1).First();
            var thirdUser = this.GetDummyUsers().Skip(2).First();
            var thirdArticle = this.GetDummyArticles().Skip(2).First();

            return new List<Comment>() {
                new Comment(body: "I like this article",username: firstUser.Username, firstArticle.Id),
                new Comment(body: "I don't like this article", secondUser.Username, firstArticle.Id), 
                new Comment(body: "I like this article but it's too short", firstUser.Username, secondArticle.Id),
                new Comment(body: "This is so weird", thirdUser.Username, secondArticle.Id),
                new Comment(body: "This is so weird and cool at the same time", thirdUser.Username, thirdArticle.Id)  
                //first user & third article will be add to test
            };
        }
    }
}