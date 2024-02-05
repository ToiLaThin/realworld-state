using Realworld.Api.Dto;

namespace Realworld.Api.Services {
    public interface IProfileService
    {
        public Task<ProfileResponseDto> GetProfileAsync(string profileUsername);
        public Task<ProfileResponseDto> FollowAsync(string profileUsername);
        public Task<ProfileResponseDto> UnfollowAsync(string profileUsername);
    }
}