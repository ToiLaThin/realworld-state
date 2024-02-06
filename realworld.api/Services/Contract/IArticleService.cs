namespace Realworld.Api.Services {
    public interface IArticleService {
        public Task<ArticleSingleResponseDto> CreateArticleAsync(CreateArticleRequestDto createArticleReq);
        public Task<ArticleSingleResponseDto> UpdateArticleAsync(string slug, UpdateArticleRequestDto updateArticleReq);

        public Task<ArticleSingleResponseDto> GetArticleBySlugAsync(string slug);

        public Task<ArticleSingleResponseDto> DeleteArticleAsync(string slug);

        public Task<ArticleSingleResponseDto> FavoriteArticleAsync(string slug);

        public Task<ArticleSingleResponseDto> UnfavoriteArticleAsync(string slug);

        public Task<ArticlesWithTotalCountDto> ListArticlesAsync(ArticlesQueryDto articlesQuery, bool isFeed = false);
    }
}