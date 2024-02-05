
using Realworld.Api.Data;

namespace Realworld.Api.Services
{
    public class TagService : ITagService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TagService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string[]> GetTagsAsync()
        {
            var tags = await _unitOfWork.TagRepository.GetTagsAsync();
            return tags.Select(t => t.Id).ToArray(); 
            //map to string id(which is tag name), this should be the work of automapper
        }
    }
}