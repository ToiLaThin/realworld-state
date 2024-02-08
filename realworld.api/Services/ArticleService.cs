
using Realworld.Api.Data;
using Realworld.Api.Extension;
using Realworld.Api.Mapping;
using Realworld.Api.Models;
using Realworld.Api.Utils;

namespace Realworld.Api.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly ICurrentUsernameAccessor _currentUsernameAccessor;

        public ArticleService(IUnitOfWork unitOfWork, ICurrentUsernameAccessor currentUsernameAccessor)
        {
            _unitOfWork = unitOfWork;
            _currentUsernameAccessor = currentUsernameAccessor;
        }

        public async Task<ArticleSingleResponseDto> CreateArticleAsync(CreateArticleRequestDto createArticleReq)
        {
            var transaction = await _unitOfWork.BeginTransactionAsync();
            string currentUsername = _currentUsernameAccessor.GetCurrentUsername();
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(currentUsername);

            // in the upsert repo, we add new tags, then return them, so we need to 
            // call savechange to get the new added tags
            var tags = await _unitOfWork.TagRepository.UpsertTagsAsync(createArticleReq.TagList);

            //the assign tag, user to article props, when add article, will also add tags and user to their table 
            // unless we tracked above, if not we may have conflict when save changes
            var article = new Article(createArticleReq.Title, createArticleReq.Description, createArticleReq.Body) {
                Author = user,
                Tags = tags.Where(t => createArticleReq.TagList.Contains(t.Id)).ToList()
            };
            _unitOfWork.ArticleRepository.AddArticle(article);

            await _unitOfWork.CommitTransactionAsync(transaction);
            bool isCurrUserFollowingArticleAuthor = article.Author.Followers.Any(f => f.FollowerName == currentUsername);
            return ArticleMapper.MapArticleToArticleSingleResponseDto(article, isCurrUserFollowingArticleAuthor);
        }

        public async Task<ArticleSingleResponseDto> GetArticleBySlugAsync(string slug)
        {
            //already have author, when we create we need to assign author, but get we don't 
            string? currentUsername = _currentUsernameAccessor.GetCurrentUsername(); //if not login, then null
            var article = await _unitOfWork.ArticleRepository.GetArticleBySlugAsync(slug, asNoTracking: true, currentUsername);
            if (article == null) {
                throw new Exception("not found article");
            }
            var comments = await _unitOfWork.CommentRepository.GetCommentsBySlugAsync(slug, currentUsername);
            article.ArticleComments = comments;

            //only work if in the get method of repo, we have include author.Followers
            // another workaround is use repo isFollowingAsync method

            bool isCurrUserFollowingArticleAuthor = currentUsername != null ?
                        article.Author.Followers.Any(f => f.FollowerName == currentUsername) :
                        false; //if not login, then following = false
            return ArticleMapper.MapArticleToArticleSingleResponseDto(article, isCurrUserFollowingArticleAuthor);
        }

        public async Task<ArticleSingleResponseDto> UpdateArticleAsync(string slug, UpdateArticleRequestDto updateArticleReq)
        {
            var transaction = await _unitOfWork.BeginTransactionAsync();
            string currentUsername = _currentUsernameAccessor.GetCurrentUsername();
            //asNoTracking: false to allow update
            var article = await _unitOfWork.ArticleRepository.GetArticleBySlugAsync(slug, asNoTracking: false, currentUsername);
            if (article == null) {
                throw new Exception("not found article");
            }

            if (article.Author.Username != currentUsername) {
                throw new Exception("not authorized");
            }

            if (string.IsNullOrEmpty(updateArticleReq.Title) ||
                string.IsNullOrEmpty(updateArticleReq.Description) ||
                string.IsNullOrEmpty(updateArticleReq.Body))
            {
                throw new Exception("invalid request");
            }

            article.Title = updateArticleReq.Title;
            article.Slug = article.Title.GenerateSlug();
            article.Description = updateArticleReq.Description;
            article.Body = updateArticleReq.Body;
            article.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.CommitTransactionAsync(transaction);
            bool isCurrUserFollowingArticleAuthor = article.Author.Followers.Any(f => f.FollowerName == currentUsername);
            return ArticleMapper.MapArticleToArticleSingleResponseDto(article, isCurrUserFollowingArticleAuthor);
        }

        public async Task<ArticleSingleResponseDto> DeleteArticleAsync(string slug)
        {
            var transaction = await _unitOfWork.BeginTransactionAsync();
            string currentUsername = _currentUsernameAccessor.GetCurrentUsername();
            var article = await _unitOfWork.ArticleRepository.GetArticleBySlugAsync(slug, asNoTracking: false, currentUsername);
            if (article == null) {
                throw new Exception("not found article");
            }

            if (article.Author.Username != currentUsername) {
                throw new Exception("not authorized");
            }

            _unitOfWork.ArticleRepository.DeleteArticle(article);
            await _unitOfWork.CommitTransactionAsync(transaction);
            bool isCurrUserFollowingArticleAuthor = article.Author.Followers.Any(f => f.FollowerName == currentUsername);
            return ArticleMapper.MapArticleToArticleSingleResponseDto(article, isCurrUserFollowingArticleAuthor);
        }

        public async Task<ArticleSingleResponseDto> FavoriteArticleAsync(string slug)
        {
            string currentUsername = _currentUsernameAccessor.GetCurrentUsername();
            var article = await _unitOfWork.ArticleRepository.GetArticleBySlugAsync(slug, asNoTracking: true, currentUsername);
            if (article == null) {
                throw new Exception("not found article");
            }
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(currentUsername);
            if (user == null) {
                throw new Exception("not found user");
            }

            var transaction = await _unitOfWork.BeginTransactionAsync();
            var articleFavoriteLink = await _unitOfWork.ArticleFavoriteRepository.GetArticleFavoriteAsync(user.Username, article.Id);
            if (articleFavoriteLink != null) {
                throw new Exception("already favorited");
            }
            _unitOfWork.ArticleFavoriteRepository.AddArticleFavorite(new ArticleFavoriteLink(article.Id, user.Username));
            await _unitOfWork.CommitTransactionAsync(transaction);

            bool isCurrUserFollowingArticleAuthor = article.Author.Followers.Any(f => f.FollowerName == currentUsername);
            return ArticleMapper.MapArticleToArticleSingleResponseDto(article, isCurrUserFollowingArticleAuthor);
        }

        public async Task<ArticleSingleResponseDto> UnfavoriteArticleAsync(string slug)
        {
            string currentUsername = _currentUsernameAccessor.GetCurrentUsername();
            var article = await _unitOfWork.ArticleRepository.GetArticleBySlugAsync(slug, asNoTracking: true, currentUsername);
            if (article == null)
            {
                throw new Exception("not found article");
            }
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(currentUsername);
            if (user == null)
            {
                throw new Exception("not found user");
            }

            var transaction = await _unitOfWork.BeginTransactionAsync();
            var articleFavoriteLink = await _unitOfWork.ArticleFavoriteRepository.GetArticleFavoriteAsync(user.Username, article.Id);
            if (articleFavoriteLink == null)
            {
                throw new Exception("not favorited");
            }
            _unitOfWork.ArticleFavoriteRepository.RemoveArticleFavorite(new ArticleFavoriteLink(article.Id, user.Username));
            await _unitOfWork.CommitTransactionAsync(transaction);

            bool isCurrUserFollowingArticleAuthor = article.Author.Followers.Any(f => f.FollowerName == currentUsername);
            return ArticleMapper.MapArticleToArticleSingleResponseDto(article, isCurrUserFollowingArticleAuthor);
        }

        public async Task<ArticlesWithTotalCountDto> ListArticlesAsync(ArticlesQueryDto articlesQuery, bool isFeed = false)
        {
            string currentUsername = _currentUsernameAccessor.GetCurrentUsername();
            var articlesResp = await _unitOfWork.ArticleRepository.GetArticlesAsync(articlesQuery, currentUsername, isFeed);
            return articlesResp;
        }
    }
}