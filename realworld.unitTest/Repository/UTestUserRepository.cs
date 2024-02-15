using FluentAssertions;
using Realworld.Api.Data;
using Realworld.Api.Models;

namespace Realworld.UnitTest.Repository
{
    public class UTestUserRepository : BaseUTestRepository
    {
        protected Realworld.Api.Data.UserRepository UserRepository { get; init; }
        public UTestUserRepository(FixtureUTestRepository fixture) : base(fixture) { 
            this.SeedDb();
            UserRepository = new Realworld.Api.Data.UserRepository(base.ConduitContext);
        }

        public override void SeedDb()
        {
            base.SeedDb();
            bool isDbJustCreated = ConduitContext.Database.EnsureCreated();
            if (ConduitContext.Users.Any()) return;
            ConduitContext.Users.AddRange(DummyUsers);
            ConduitContext.SaveChanges();
        }

        [Theory]
        [InlineData("user1")]
        [InlineData("user2")]
        [InlineData("user3")]
        public async Task WhenGetUserByValidUsername_ReturnValidUser(string username)
        {
            var actualUser = await UserRepository.GetUserByUsernameAsync(username);
            var expectedUser = base.DummyUsers.FirstOrDefault(u => u.Username == username);
            Assert.NotNull(actualUser);
            expectedUser.Should().BeEquivalentTo(actualUser);
            expectedUser.Should().BeOfType<User>();
        }

        [Theory]
        [InlineData("user5")]
        public async Task WhenGetUserByInValidUsername_ReturnNull(string username)
        {
            var actualUser = await UserRepository.GetUserByUsernameAsync(username);
            Assert.Null(actualUser);
        }


        [Theory]
        [InlineData("user1@gmail.com")]
        [InlineData("user2@gmail.com")]
        [InlineData("user3@gmail.com")]
        public async Task WhenGetUserByValidEmail_ReturnValidUser(string email)
        {
            var actualUser = await UserRepository.GetUserByEmailAsync(email);
            var expectedUser = base.DummyUsers.FirstOrDefault(u => u.Email == email);
            Assert.NotNull(actualUser);
            expectedUser.Should().BeEquivalentTo(actualUser);
            expectedUser.Should().BeOfType<User>();
        }

        [Fact]
        public async Task WhenAddUser_VerifyCorrectUserAdded()
        {
            //Arrange
            var addedUser = new User() {
                    Username = "user4",
                    Email = "user4@gmail.com",
                    Password = "user4",
                    Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c",
                    Bio = String.Empty,
                    Image = String.Empty,
            };

            //Act
            await UserRepository.AddUserAsync(addedUser);
            base.ConduitContext.SaveChanges();

            //Assert
            var actualUser = await UserRepository.GetUserByUsernameAsync(addedUser.Username);
            Assert.NotNull(actualUser);
            actualUser.Should().BeEquivalentTo(addedUser);
            actualUser.Should().BeOfType<User>();
        }

    }
}