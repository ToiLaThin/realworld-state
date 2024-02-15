using Npgsql.Replication;
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
    }
}