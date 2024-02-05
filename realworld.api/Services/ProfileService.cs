using Realworld.Api.Data;
using Realworld.Api.Dto;
using Realworld.Api.Utils;

namespace Realworld.Api.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUsernameAccessor _currentUsernameAccessor;
        public ProfileService(IUnitOfWork unitOfWork, ICurrentUsernameAccessor currentUsernameAccessor)
        {
            _unitOfWork = unitOfWork;
            _currentUsernameAccessor = currentUsernameAccessor;
        }


        public async Task<ProfileResponseDto> FollowAsync(string profileUsername)
        {
            var transaction = await _unitOfWork.BeginTransactionAsync();
            var profileUser = await _unitOfWork.UserRepository.GetUserByUsernameAsync(profileUsername);
            if (profileUser is null)
            {
                throw new Exception("profile user not found");
            }
            // this require [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] in controller calling this service method
            string currentUsername = _currentUsernameAccessor.GetCurrentUsername();
            _unitOfWork.UserRepository.Follow(profileUsername, currentUsername);
            await _unitOfWork.CommitTransactionAsync(transaction);

            return new ProfileResponseDto(profileUser.Username, profileUser.Bio, profileUser.Image, true);
        }

        public async Task<ProfileResponseDto> GetProfileAsync(string profileUsername)
        {
            var profileUser = await _unitOfWork.UserRepository.GetUserByUsernameAsync(profileUsername);
            if (profileUser is null)
            {
                throw new Exception("profile user not found");
            }

            string currentUsername = _currentUsernameAccessor.GetCurrentUsername();
            bool IsCurrentUserFollowedProfile = await _unitOfWork.UserRepository.IsFollowingAsync(profileUsername, currentUsername);
            return new ProfileResponseDto(profileUser.Username, profileUser.Bio, profileUser.Image, IsCurrentUserFollowedProfile);
        }

        public async Task<ProfileResponseDto> UnfollowAsync(string profileUsername)
        {
            var transaction = await _unitOfWork.BeginTransactionAsync();
            var profileUser = await _unitOfWork.UserRepository.GetUserByUsernameAsync(profileUsername);
            if (profileUser is null)
            {
                throw new Exception("profile user not found");
            }
            string currentUsername = _currentUsernameAccessor.GetCurrentUsername();
            _unitOfWork.UserRepository.Unfollow(profileUsername, currentUsername);
            await _unitOfWork.CommitTransactionAsync(transaction);

            return new ProfileResponseDto(profileUser.Username, profileUser.Bio, profileUser.Image, false);
        }
    }
}