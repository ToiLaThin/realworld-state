using Realworld.Api.Models;
using Realworld.Api.Dto;
using Realworld.Api.Data;

namespace Realworld.Api.Mapping {
    public static class CommentMapper {
        /// <summary>
        /// For readability, we can put return CommentSingleResponseDto directly in the service method
        /// We can use automapper but since this is class to record mapping, require params, create a custom one is better.
        /// This is a factory method to create CommentSingleResponseDto from Comment and isFollowing (get this from service to avoid more dependency)
        /// </summary>
        /// <param name="comment"></param>
        /// <param name="isFollowing"></param>
        /// <returns></returns>
        public static CommentSingleResponseDto MapCommentToCommentSingleResponseDto(Comment comment, bool isFollowing) {
            return new CommentSingleResponseDto(
                comment.Id,
                comment.CreatedAt,
                comment.UpdatedAt,
                comment.Body,
                new ProfileResponseDto(
                    comment.Author.Username,
                    comment.Author.Bio,
                    comment.Author.Image,
                    isFollowing
                )
            );
        }
    }
}