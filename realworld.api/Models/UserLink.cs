namespace Realworld.Api.Models {
    public class UserLink
    {
        public string UserName { get; set; }
        public User User { get; set; }
        public string FollowerName { get; set; }
        public User Follower { get; set; }

        public UserLink() {}
    }
}
