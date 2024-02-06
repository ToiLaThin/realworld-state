using Realworld.Api.Dto;

namespace Realworld.Api.Services {
    public interface ICommentService {
        public Task<List<CommentSingleResponseDto>> GetCommentsFromArticleAsync(string slug);

        public Task<CommentSingleResponseDto> AddCommentToArticleAsync(string slug, CreateCommentRequestDto createCommentReq);

        public Task DeleteCommentAsync(string slug, int commentId);
    }
}