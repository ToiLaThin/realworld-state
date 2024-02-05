using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Realworld.Api.Dto;
using Realworld.Api.Services;

namespace Realworld.Api.Controllers
{
    /// <summary>
    /// Envelope for profile response, will be json serialized as {"profile": { ... }} if is Profile not Data
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Profile"></param>
    public record ProfileEnvelope<T>(T Profile);


    [ApiController]
    public class ProfileController: ControllerBase {
        private readonly IProfileService _profileService;
        public ProfileController(IProfileService profileService) {
            _profileService = profileService;
        }

        [HttpGet("api/profiles/{profileUsername}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ProfileEnvelope<ProfileResponseDto>> GetProfileAsync(string profileUsername) {
            var profileResponse = await _profileService.GetProfileAsync(profileUsername);
            return new ProfileEnvelope<ProfileResponseDto>(profileResponse);
        }

        /// <summary>
        /// Follow a profile, that profile can be current user too, (current user follow itself)
        /// </summary>
        /// <param name="profileUsername"></param>
        /// <returns></returns>
        [HttpPost("api/profiles/{profileUsername}/follow")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ProfileEnvelope<ProfileResponseDto>> FollowProfileAsync([FromRoute] string profileUsername) {
            var profileResponse = await _profileService.FollowAsync(profileUsername);
            return new ProfileEnvelope<ProfileResponseDto>(profileResponse);
        }

        [HttpDelete("api/profiles/{profileUsername}/follow")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ProfileEnvelope<ProfileResponseDto>> UnfollowProfileAsync([FromRoute] string profileUsername) {
            var profileResponse = await _profileService.UnfollowAsync(profileUsername);
            return new ProfileEnvelope<ProfileResponseDto>(profileResponse);
        }
    }
}