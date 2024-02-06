using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Realworld.Api.Dto;
using Realworld.Api.Services;

namespace Realworld.Api.Controllers {
    public record CommentSingleEnvelope<T>(T Comment);
    public record CommentsMultipleEnvelope<T>(List<T> Comments);

    [ApiController]
    public class CommentController: ControllerBase {
        private readonly ICommentService _commentService;
        public CommentController(ICommentService commentService) {
            _commentService = commentService;
        }

        [HttpGet("api/articles/{slug}/comments")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<CommentsMultipleEnvelope<CommentSingleResponseDto>> GetCommentsFromArticleAsync(string slug) {
            var commentsResp = await _commentService.GetCommentsFromArticleAsync(slug);
            return new CommentsMultipleEnvelope<CommentSingleResponseDto>(commentsResp);
        }

        [HttpPost("api/articles/{slug}/comments")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<CommentSingleEnvelope<CommentSingleResponseDto>> AddCommentToArticleAsync(string slug,[FromBody] CreateCommentRequestDto createCommentReq) {
            var commentResp = await _commentService.AddCommentToArticleAsync(slug, createCommentReq);
            return new CommentSingleEnvelope<CommentSingleResponseDto>(commentResp);
        }

        [HttpDelete("api/articles/{slug}/comments/{commentId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task DeleteCommentAsync(string slug, int commentId) {
            await _commentService.DeleteCommentAsync(slug, commentId);
        }

    }
}