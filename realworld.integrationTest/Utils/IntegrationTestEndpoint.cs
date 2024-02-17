namespace Realworld.IntegrationTest.Utils
{
    public static class IntegrationTestEndpoint {
        public const string BaseUrl = "http://localhost:5159/api";

        public static class Article {
            public const string GetArticleBySlug = $"{BaseUrl}/articles/";
            public const string ListArticles = $"{BaseUrl}/articles";

        }
    }
}