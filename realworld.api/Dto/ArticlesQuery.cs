using Realworld.Api.Models;

public record ArticlesQuery(
    string? Tag,
    string? Author,
    string? Favorited,
    int Limit = 20,
    int Offset = 0
);


public record ArticlesResponseDto(
    List<Article> Articles,
    int ArticlesCount
);