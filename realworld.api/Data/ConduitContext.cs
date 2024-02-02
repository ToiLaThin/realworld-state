using Microsoft.EntityFrameworkCore;
using Realworld.Api.Models;

namespace Realworld.Api.Data
{
  public class ConduitContext : DbContext
  {
    public ConduitContext(DbContextOptions<ConduitContext> options) : base(options) { }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Article> Articles { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<Tag> Tags { get; set; } = null!;
    public DbSet<ArticleFavoriteLink> ArticleFavoriteLinks { get; set; } = null!;
    public DbSet<UserLink> UserLinks { get; set; } = null!;


    //instead of making 6 class of small config for each entity, we can use this method to config all entities
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      //specify model builder for every entity
      modelBuilder.Entity<User>(entity =>
      {
        entity.ToTable("Users");
        entity.HasKey(e => e.Username);
        entity.HasIndex(e => e.Email).IsUnique();
        entity.HasMany(u => u.CommentsAboutArticle).WithOne(c => c.Author);
      });

      modelBuilder.Entity<Comment>(entity =>
      {
        entity.ToTable("Comments");
        entity.HasKey(e => e.Id);
        //config navigation properties and foreign key
        entity.HasOne(c => c.Author).WithMany(u => u.CommentsAboutArticle).HasForeignKey(c => c.Username);
        entity.HasOne(c => c.Article).WithMany(a => a.ArticleComments).HasForeignKey(c => c.ArticleId);
      });

      modelBuilder.Entity<ArticleFavoriteLink>(entity =>
      {
        entity.ToTable("ArticleFavoriteLinks");
        entity.HasKey(e => new { e.ArticleId, e.Username });
        entity.HasOne(afl => afl.Article).WithMany(a => a.UserFavoritesArticleMappings).HasForeignKey(afl => afl.ArticleId);
        entity.HasOne(afl => afl.User).WithMany(u => u.ArticlesFavoritedByUserMapping).HasForeignKey(afl => afl.Username);
      });


      //need fkey + nav prop for many to many 
      modelBuilder.Entity<UserLink>(entity =>
      {
        entity.ToTable("UserFollower");
        entity.HasKey(e => new { e.UserName, e.FollowerName });
        //config 1 follower will follow many users
        entity.HasOne(ul => ul.Follower).WithMany(u => u.FollowedUsers).HasForeignKey(ul => ul.FollowerName);
        //config 1 user will be followed by many followers of model User
        entity.HasOne(ul => ul.User).WithMany(u => u.Followers).HasForeignKey(ul => ul.UserName);
      });

      modelBuilder.Entity<Article>(entity =>
      {
        entity.ToTable("Articles");
        entity.HasKey(e => e.Id);
        entity.HasIndex(e => e.Slug).IsUnique();
        entity.Ignore(e => e.Favorited);
        entity.Ignore(e => e.FavoritesCount);
      });
    }

  }
}