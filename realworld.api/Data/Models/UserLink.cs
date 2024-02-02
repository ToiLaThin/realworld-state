namespace Realworld.Api.Models {
    public class UserLink
    {
        public string UserName { get; set; }
        public User User { get; set; } = null!;
        public string FollowerName { get; set; }
        public User Follower { get; set; } = null!;

        public UserLink() {}
        public UserLink(string username, string followerName) {
            UserName = username;
            FollowerName = followerName;
        }
    }
}
