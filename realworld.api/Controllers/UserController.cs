using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Realworld.Api.Data;
using Realworld.Api.Dto;
using Realworld.Api.Services;
using Realworld.Api.Utils;
using System.Reflection;
using System.Security.Claims;

namespace Realworld.Api.Controllers
{
    [ApiController]
    public class UserController: ControllerBase {

        private readonly IUserService _userService;
        private readonly ICurrentUsernameAccessor _currentUsernameAccessor;

        public UserController(ICurrentUsernameAccessor currentUsernameAccessor, IUserService userService) {
            _currentUsernameAccessor = currentUsernameAccessor;
            _userService = userService;
        }

        [HttpPost("api/users/login")]
        public async Task<UserEnvelope<UserResponseDto>> Authenticate(RequestEnvelope<UserEnvelope<LoginUserRequestDto>> requestEnvelope) {
            var loginUserReq = requestEnvelope.Body.User;
            var userRespone = await _userService.LoginAsync(loginUserReq);
            return new UserEnvelope<UserResponseDto>(userRespone);
        }

        [HttpPost("api/users")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<UserEnvelope<UserResponseDto>> Registration(RequestEnvelope<UserEnvelope<CreateUserRequestDto>> requestEnvelope) {
            var registerUserReq = requestEnvelope.Body.User;
            var userResponse = await _userService.CreateAsync(registerUserReq);
            return new UserEnvelope<UserResponseDto>(userResponse);
        }

        [HttpGet("api/user")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<UserEnvelope<UserResponseDto>> GetCurrentUser() {
            var username = _currentUsernameAccessor.GetCurrentUsername();
            var userResponse = await _userService.GetAsync(username);
            return new UserEnvelope<UserResponseDto>(userResponse);
        }

        [HttpPut("api/user")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<UserEnvelope<UserResponseDto>> UpdateUser(RequestEnvelope<UserEnvelope<UpdateUserRequestDto>> requestEnvelope) {
            var updateUserReq = requestEnvelope.Body.User;
            var userResponse = await _userService.UpdateAsync(updateUserReq);
            return new UserEnvelope<UserResponseDto>(userResponse);
        }
    }
}