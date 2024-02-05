using Microsoft.EntityFrameworkCore.Storage;

namespace Realworld.Api.Data {
    public interface IUnitOfWork {
        public IUserRepository UserRepository { get; }

        public IArticleRepository ArticleRepository { get; }

        public ICommentRepository CommentRepository { get; }

        public ITagRepository TagRepository { get; }

        public IArticleFavoriteRepository ArticleFavoriteRepository { get; }

        public IDbContextTransaction GetCurrentTransaction();
        public Task CommitTransactionAsync(IDbContextTransaction transaction);

        public Task RollbackTransactionAsync();

        public Task<IDbContextTransaction> BeginTransactionAsync();

        public bool HasActiveTransaction ();

    }
}