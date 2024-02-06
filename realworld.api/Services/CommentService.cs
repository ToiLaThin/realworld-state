using Realworld.Api.Data;
using Realworld.Api.Dto;
using Realworld.Api.Models;
using Realworld.Api.Utils;

namespace Realworld.Api.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly ICurrentUsernameAccessor _currentUsernameAccessor;

        public CommentService(IUnitOfWork unitOfWork, ICurrentUsernameAccessor currentUsernameAccessor)
        {
            _unitOfWork = unitOfWork;
            _currentUsernameAccessor = currentUsernameAccessor;
        }

        public async Task<CommentSingleResponseDto> AddCommentToArticleAsync(string slug, CreateCommentRequestDto createCommentReq)
        {
            string currentUsername = _currentUsernameAccessor.GetCurrentUsername();
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(currentUsername);
            var article = await _unitOfWork.ArticleRepository.GetArticleBySlugAsync(slug, asNoTracking: false);
            if (article == null)
            {
                throw new Exception("not found article with" + slug);
            }
            var transaction = await _unitOfWork.BeginTransactionAsync();

            var toAddComment = new Comment(
                body: createCommentReq.Body, 
                username: currentUsername, 
                articleId: article.Id
            );

            _unitOfWork.CommentRepository.AddArticleComment(toAddComment);
            await _unitOfWork.CommitTransactionAsync(transaction);
            return new CommentSingleResponseDto(
                toAddComment.Id,
                toAddComment.CreatedAt,
                toAddComment.UpdatedAt,
                toAddComment.Body,
                new ProfileResponseDto(
                    user.Username,
                    user.Bio,
                    user.Image,
                    await _unitOfWork.UserRepository.IsFollowingAsync(user.Username, currentUsername) //is author (current user) is followed by himself
                )
            );
            
        }

        public async Task DeleteCommentAsync(string slug, int commentId)
        {
            var article = await _unitOfWork.ArticleRepository.GetArticleBySlugAsync(slug, asNoTracking: false);
            if (article == null) {
                throw new Exception("not found article with" + slug);
            }

            var comments = await _unitOfWork.CommentRepository.GetCommentsBySlugAsync(slug, null);
            var commentToDel = comments.FirstOrDefault(c => c.Id == commentId);
            if (commentToDel == null) {
                throw new Exception("not found comment with" + commentId);
            }

            var transaction = await _unitOfWork.BeginTransactionAsync();
            _unitOfWork.CommentRepository.RemoveArticleComment(commentToDel);
            await _unitOfWork.CommitTransactionAsync(transaction);
        }

        public async Task<List<CommentSingleResponseDto>> GetCommentsFromArticleAsync(string slug)
        {
            string currentUsername = _currentUsernameAccessor.GetCurrentUsername(); //can be null
            var comments = await _unitOfWork.CommentRepository.GetCommentsBySlugAsync(slug, currentUsername);
            var commentsResp = new List<CommentSingleResponseDto>();
            foreach (var comment in comments) {
                var commentSingleResp = new CommentSingleResponseDto(
                    comment.Id,
                    comment.CreatedAt,
                    comment.UpdatedAt,
                    comment.Body,
                    new ProfileResponseDto(
                        comment.Author.Username,
                        comment.Author.Bio,
                        comment.Author.Image,
                        await _unitOfWork.UserRepository.IsFollowingAsync(comment.Author.Username, currentUsername)
                    ));
                commentsResp.Add(commentSingleResp);
            }
            return commentsResp;
        }
    }
}