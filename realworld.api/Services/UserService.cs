using System.Net;
using Microsoft.IdentityModel.Tokens;
using Realworld.Api.Data;
using Realworld.Api.Dto;
using Realworld.Api.Models;
using Realworld.Api.Utils;
using Realworld.Api.Utils.ExceptionHandling;

namespace Realworld.Api.Services {
    public class UserService: IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        public UserService(IUnitOfWork unitOfWork, IJwtTokenGenerator jwtTokenGenerator)
        {
            _unitOfWork = unitOfWork;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<UserResponseDto> CreateAsync(CreateUserRequestDto createUserReq)
        {
            var transaction = await _unitOfWork.BeginTransactionAsync();
            var token = _jwtTokenGenerator.GenerateToken(createUserReq.Username);
            var newUser = new User() {
                Username = createUserReq.Username,
                Email = createUserReq.Email,
                Password = createUserReq.Password,
                Token = token,
                Bio = String.Empty,
                Image = String.Empty,
            };

            await _unitOfWork.UserRepository.AddUserAsync(newUser);
            //can use auto mapper here
            await _unitOfWork.CommitTransactionAsync(transaction);
            return new UserResponseDto(newUser.Username, newUser.Email, token, newUser.Bio, newUser.Image);
        }

    public async Task<UserResponseDto> GetAsync(string username)
    {
        var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
        if (user == null) {
            throw new ConduitException(HttpStatusCode.Forbidden, new { User = ConduitErrors.UNAUTHORIZED });
        }
        return new UserResponseDto(user.Username, user.Email, user.Token, user.Bio, user.Image);
    }

    public async Task<UserResponseDto> LoginAsync(LoginUserRequestDto userLoginReq)
        {
            var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(userLoginReq.Email);
            if (user == null || user.Password != userLoginReq.Password) {
                throw new ConduitException(HttpStatusCode.Forbidden, new { User = ConduitErrors.UNAUTHORIZED });
            }
            return new UserResponseDto(user.Username, user.Email, user.Token, user.Bio, user.Image);
        }

        public async Task<UserResponseDto> UpdateAsync(UpdateUserRequestDto userUpdateReq)
        {
            var transaction = await _unitOfWork.BeginTransactionAsync();
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(userUpdateReq.Username);
            if (userUpdateReq.Username != user.Username) {
                user.Token = _jwtTokenGenerator.GenerateToken(user.Username);
            }
            user.Username = userUpdateReq.Username;
            user.Email = userUpdateReq.Email;
            user.Password = userUpdateReq.Password;
            user.Bio = userUpdateReq.Bio;
            user.Image = userUpdateReq.Image;
            await _unitOfWork.UserRepository.UpdateUserAsync(user);
            await _unitOfWork.CommitTransactionAsync(transaction);
            return new UserResponseDto(user.Username, user.Email, user.Token, user.Bio, user.Image);
        }

  }
}

