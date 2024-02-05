namespace Realworld.Api.Services {
    public interface ITagService {
        public Task<string[]> GetTagsAsync();
    }
}