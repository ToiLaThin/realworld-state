using System.IdentityModel.Tokens.Jwt;
using System.Text;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Realworld.Api.Utils;

namespace Realworld.UnitTest.Utils {
    public class UTestJwtTokenGenerator {
        protected Realworld.Api.Utils.JwtTokenGenerator JwtTokenGenerator { get; set; }

        private JwtTokenGenerator CreateJwtTokenGenerator(string audience, string issuer, string secret) {
            var jwtOptions = Options.Create(new JwtOptions() {
                Audience = audience,
                Issuer = issuer,
                Secret = secret
            });
            
            return new Realworld.Api.Utils.JwtTokenGenerator(jwtOptions);
        }
        public UTestJwtTokenGenerator() {            
            JwtTokenGenerator = CreateJwtTokenGenerator(
                issuer: "https://localhost:5001",
                audience: "https://localhost:5001",
                secret: "supersecret have to have the required length"
            );
        }

        [Theory]
        [InlineData("user1", "https://localhost:5001", "https://localhost:5001", "supersecret have to have the required length")]
        [InlineData("user2", ".net core realworld api", "angular client", "supersecret have to have the required length 2")]
        public void WhenGenerateToken_ReturnValidToken(string username, string issuer, string audience, string secret) {
            // Arrange
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            // var expectedKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            // var expectedCredential = new SigningCredentials(expectedKey, SecurityAlgorithms.HmacSha256Signature);
            JwtTokenGenerator = CreateJwtTokenGenerator(audience, issuer, secret);

            // Act
            var tokenGenerated = JwtTokenGenerator.GenerateToken(username);
            var tokenCasted = tokenHandler.ReadToken(tokenGenerated) as JwtSecurityToken;

            // Assert
            Assert.NotNull(tokenCasted);
            tokenCasted.Issuer.Should().Be(issuer);
            tokenCasted.Audiences.Should().BeEquivalentTo(new string[] { audience });
            tokenCasted.Subject.Should().Be(username);
        }
    }
}