using Microsoft.EntityFrameworkCore;
using Realworld.Api.Models;

namespace Realworld.Api.Data
{
  public class UserRepository : IUserRepository
  {
    private readonly ConduitContext _context;
    public UserRepository(ConduitContext context) { 
        _context = context;
    }
    public async Task AddUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }


    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
      return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<bool> IsFollowingAsync(string username, string followerName)
    {
        return await _context.UserLinks.AnyAsync(ul => ul.UserName == username && ul.FollowerName == followerName);
    }

    public void Follow(string username, string followerName)
    {
        _context.UserLinks.Add(new UserLink { UserName = username, FollowerName = followerName });
    }

    public void Unfollow(string username, string followerName)
    {
        _context.UserLinks.Remove(new UserLink { UserName = username, FollowerName = followerName });
    }
  }
}