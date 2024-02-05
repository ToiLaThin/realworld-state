
using Microsoft.EntityFrameworkCore.Storage;

namespace Realworld.Api.Data {
  public class UnitOfWork : IUnitOfWork
  {
    private readonly ConduitContext _context;
    private IUserRepository _userRepository;
    private IArticleRepository _articleRepository;
    private ICommentRepository _commentRepository;
    private IArticleFavoriteRepository _articleFavoriteRepository;
    private ITagRepository _tagRepository;

    private IDbContextTransaction _currentTransaction = null;
    public UnitOfWork(ConduitContext context)
    {
      _context = context;
    }

    public IUserRepository UserRepository {
        get {
            if (_userRepository == null) {
                _userRepository = new UserRepository(_context);
            }
            return _userRepository;
        }
    }

    public IArticleRepository ArticleRepository {
        get {
            if (_articleRepository == null) {
                _articleRepository = new ArticleRepository(_context);
            }
            return _articleRepository;
        }
    }

    public ICommentRepository CommentRepository {
        get {
            if (_commentRepository == null) {
                _commentRepository = new CommentRepository(_context);
            }
            return _commentRepository;
        }
    }

    public ITagRepository TagRepository {
        get {
            if (_tagRepository == null) {
                _tagRepository = new TagRepository(_context);
            }
            return _tagRepository;
        }
    }

    public IArticleFavoriteRepository ArticleFavoriteRepository {
        get {
            if (_articleFavoriteRepository == null) {
                _articleFavoriteRepository = new ArticleFavoriteRepository(_context);
            }
            return _articleFavoriteRepository;
        }
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        if (_currentTransaction != null) {
            return null;
        }
        _currentTransaction = await _context.Database.BeginTransactionAsync();
        return _currentTransaction;
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction)
    {
        if (transaction == null) {
            throw new ArgumentNullException(nameof(transaction));
        }

        try {
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        } 
        catch {
            await transaction.RollbackAsync();
            throw;
        } 
        finally {
            if (_currentTransaction != null) {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }

    }

    public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;
    

    public bool HasActiveTransaction() => _currentTransaction != null;

    public async Task RollbackTransactionAsync()
    {
        if (_currentTransaction == null) {
            throw new InvalidOperationException($"No active transaction");
        }
        try {
            await _currentTransaction.RollbackAsync();
        } 
        finally {
            if (_currentTransaction != null) {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }
  }
}