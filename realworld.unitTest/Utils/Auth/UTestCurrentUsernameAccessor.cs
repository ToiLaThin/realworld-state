using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Realworld.UnitTest.Utils
{
    public class UTestCurrentUsernameAccessor
    {
        public UTestCurrentUsernameAccessor() {}

        [Theory]
        [InlineData("user1")]
        [InlineData("user2")]
        [InlineData("user3")]
        public void WhenGetCurrentUsername_ReturnValidUsername(string username)
        {
            // Arrange
            Mock<IHttpContextAccessor> mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.Setup(x => x.HttpContext.User.Claims).Returns(new Claim[] { new Claim(ClaimTypes.NameIdentifier, username) });
            var currentUsernameAccessor = new Realworld.Api.Utils.CurrentUsernameAccessor(mockHttpContextAccessor.Object); // SUT

            var actualUsername = currentUsernameAccessor.GetCurrentUsername();
            Assert.Equal(username, actualUsername);
        }
    }
}