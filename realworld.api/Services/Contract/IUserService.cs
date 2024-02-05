using Realworld.Api.Dto;

namespace Realworld.Api.Services {
    public interface IUserService
    {
        public Task<UserResponseDto> CreateAsync(CreateUserRequestDto userCreateReq);
        public Task<UserResponseDto> UpdateAsync(UpdateUserRequestDto userUpdateReq);

        public Task<UserResponseDto> LoginAsync(LoginUserRequestDto userLoginReq);

        public Task<UserResponseDto> GetAsync(string username);
    }
}

