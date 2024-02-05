using Realworld.Api.Models;

namespace Realworld.Api.Data
{
    public interface IUserRepository {
        public Task<User?> GetUserByEmailAsync(string email);
        public Task<User?> GetUserByUsernameAsync(string username);
        public Task AddUserAsync(User user);

        public Task UpdateUserAsync(User user);

        public Task<bool> IsFollowingAsync(string username, string followerName);

        public void Follow(string username, string followerName);

        public void Unfollow(string username, string followerName);
    }
}