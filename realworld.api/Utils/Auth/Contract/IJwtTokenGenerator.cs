namespace Realworld.Api.Utils
{
    public interface IJwtTokenGenerator
    {
        public string GenerateToken(string username);
    }
}