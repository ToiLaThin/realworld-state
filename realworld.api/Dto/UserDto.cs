using System.ComponentModel.DataAnnotations;

namespace Realworld.Api.Dto {
    public record UserResponseDto(string Username, string Email, string Token, string Bio, string Image);
    public record CreateUserRequestDto(string Username, string Email, string Password);
    public record UpdateUserRequestDto(string Email, string Username, string Password, string Image, string Bio);
    public record LoginUserRequestDto(string Email, string Password);      
}
