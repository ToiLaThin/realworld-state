using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Realworld.Api.Data;
using Realworld.Api.Models;

namespace Realworld.UnitTest.Repository
{
    public class UTestUserRepository : BaseUTestRepository
    {
        protected Realworld.Api.Data.UserRepository UserRepository { get; init; }
        public UTestUserRepository(FixtureUTestRepository fixture) : base(fixture) { 
            base.SeedDb();
            UserRepository = new Realworld.Api.Data.UserRepository(base.ConduitContext);
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
            expectedUser.CommentsAboutArticle = null; //because the prepared data in DummyDataProvider has no CommentsAboutArticle
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

        [Fact]
        public async Task WhenUpdateUser_VerifyCorrectUserUpdated()
        {
            //Arrange
            var existingUser = base.ConduitContext.Users.First();
            existingUser.Bio = "new bio";
            existingUser.Image = "new image";

            //Act
            await UserRepository.UpdateUserAsync(existingUser);
            base.ConduitContext.SaveChanges();

            //Assert
            var actualUser = await UserRepository.GetUserByUsernameAsync(existingUser.Username);
            Assert.NotNull(actualUser);
            actualUser.Should().BeEquivalentTo(existingUser);
            actualUser.Should().BeAssignableTo<User>();
        }

        [Fact]
        public async Task WhenVerifyTwoUnfollowedUser_ReturnUserNotFollowed()
        {
            //Arrange
            var firstUserToBeFollowed = base.ConduitContext.Users.First();
            var secondUserToFollowed = base.ConduitContext.Users.Skip(1).First();

            //Act
            var isFirstUserFollowedSecondUser = await UserRepository.IsFollowingAsync(firstUserToBeFollowed.Username, secondUserToFollowed.Username);

            //Assert
            Assert.False(isFirstUserFollowedSecondUser);
        }

        [Fact]
        public async Task WhenFirstUserFollowedBySeconduser_ReturnFollowedResult()
        {
            //Arrange
            var firstUserToBeFollowed = base.ConduitContext.Users.First();
            var secondUserToFollowed = base.ConduitContext.Users.Skip(1).First();

            //Act
            UserRepository.Follow(firstUserToBeFollowed.Username, secondUserToFollowed.Username);
            base.ConduitContext.SaveChanges();

            //Assert
            var isFirstUserFollowedSecondUser = await UserRepository.IsFollowingAsync(firstUserToBeFollowed.Username, secondUserToFollowed.Username);
            Assert.True(isFirstUserFollowedSecondUser);
            //avoid data inconsistency with other tests
            ConduitContext.UserLinks.Entry(new UserLink() { UserName = firstUserToBeFollowed.Username, FollowerName = secondUserToFollowed.Username }).State = EntityState.Deleted;
            await ConduitContext.SaveChangesAsync();
        }

        [Fact]
        public async Task GivenFirstUserFollowedBySecondUser_WhenUnfollow_ReturnNotFollowedResult()
        {
            //Arrange
            var transaction = await ConduitContext.Database.BeginTransactionAsync();
            var firstUserToBeFollowed = base.ConduitContext.Users.First();
            var secondUserToFollowed = base.ConduitContext.Users.Skip(1).First();
            UserRepository.Follow(firstUserToBeFollowed.Username, secondUserToFollowed.Username);
            base.ConduitContext.SaveChanges();

            //Act
            UserRepository.Unfollow(firstUserToBeFollowed.Username, secondUserToFollowed.Username);
            base.ConduitContext.SaveChanges();
            await transaction.CommitAsync();

            //Assert
            var isFirstUserFollowedSecondUser = await UserRepository.IsFollowingAsync(firstUserToBeFollowed.Username, secondUserToFollowed.Username);
            Assert.False(isFirstUserFollowedSecondUser);
        }
    }
}