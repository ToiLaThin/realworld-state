using Realworld.Api.Dto;

namespace Realworld.Api.Dto {
    public record CommentSingleResponseDto(
        int Id, 
        DateTime CreatedAt,
        DateTime UpdatedAt, 
        string Body,
        ProfileResponseDto Author
    );

    public record CreateCommentRequestDto(
        string Body
    );

}