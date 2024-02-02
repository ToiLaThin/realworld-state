using Realworld.Api.Models;

namespace Realworld.Api.Data
{
    public interface ITagRepository {
        public Task<IEnumerable<Tag>> UpsertTagsAsync(IEnumerable<string> tags);

        public Task<List<Tag>> GetTagsAsync();
    }
}