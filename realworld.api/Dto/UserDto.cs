using System.ComponentModel.DataAnnotations;

namespace Realworld.Api.Dto {
    public record UserResponseDto(string Username, string Email, string Token, string Bio, string Image);
    public record CreateUserRequestDto(string Username, string Email, string Password);

    //TODO: add validators for these
    public record UpdateUserRequestDto(string? Email = null, string? Username = null, string? Password = null, string? Image = null, string? Bio = null);
    public record LoginUserRequestDto(string Email, string Password);      
}
