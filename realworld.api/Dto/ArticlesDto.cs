using Realworld.Api.Dto;
using Realworld.Api.Models;

public record ArticlesQueryDto(
    string? Tag,
    string? Author,
    string? Favorited,
    int Limit = 20,
    int Offset = 0
);

public record ArticlesFeedQueryDto(
    int Limit = 20,
    int Offset = 0
);

public record CreateArticleRequestDto(
    string Title,
    string Description,
    string Body,
    IEnumerable<string> TagList
);

public record UpdateArticleRequestDto(
    string? Title,
    string? Description,
    string? Body
);

/// <summary>
/// This hold multiple articles and total count of articles 
/// to map to multiple ArticlesSingleResponseDto and put in ArticlesMultipleEnvelope
/// </summary>
/// <param name="Articles"></param>
/// <param name="ArticlesCount"></param>
public record ArticlesWithTotalCountDto(
    List<Article> Articles,
    int ArticlesCount
);

public record ArticleSingleResponseDto(
    string Slug,
    string Title,
    string Description,
    string Body,
    IEnumerable<string> TagList,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    bool Favorited,
    int FavoritesCount,
    ProfileResponseDto Author //this dto is defined in ProfileDto.cs, same structure so we reuse it

);



