using System.Net;
using System.Runtime.CompilerServices;
using Realworld.Api.Data;
using Realworld.Api.Dto;
using Realworld.Api.Mapping;
using Realworld.Api.Models;
using Realworld.Api.Utils;
using Realworld.Api.Utils.ExceptionHandling;

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
            if (article == null) {
                throw new ConduitException(HttpStatusCode.NotFound, new { Article = ConduitErrors.NOT_FOUND });
            }
            var transaction = await _unitOfWork.BeginTransactionAsync();

            var toAddComment = new Comment(
                body: createCommentReq.Body, 
                username: currentUsername, 
                articleId: article.Id
            );

            _unitOfWork.CommentRepository.AddArticleComment(toAddComment);
            await _unitOfWork.CommitTransactionAsync(transaction);
            bool isUserFollowingHimself = await _unitOfWork.UserRepository.IsFollowingAsync(user.Username, currentUsername); //is author (current user) is followed by himself
            return CommentMapper.MapCommentToCommentSingleResponseDto(toAddComment, isUserFollowingHimself);
            
        }

        public async Task DeleteCommentAsync(string slug, int commentId)
        {
            var article = await _unitOfWork.ArticleRepository.GetArticleBySlugAsync(slug, asNoTracking: false);
            if (article == null) {
                throw new ConduitException(HttpStatusCode.NotFound, new { Article = ConduitErrors.NOT_FOUND });
            }

            var comments = await _unitOfWork.CommentRepository.GetCommentsBySlugAsync(slug, null);
            var commentToDel = comments.FirstOrDefault(c => c.Id == commentId);
            if (commentToDel == null) {
                throw new ConduitException(HttpStatusCode.NotFound, new { Comment = ConduitErrors.NOT_FOUND });
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
                bool isCurrUserFollowingCommentAuthor = await _unitOfWork.UserRepository.IsFollowingAsync(comment.Author.Username, currentUsername);
                var commentSingleResp = CommentMapper.MapCommentToCommentSingleResponseDto(comment, isCurrUserFollowingCommentAuthor);
                commentsResp.Add(commentSingleResp);
            }
            return commentsResp;
        }
    }
}