using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Realworld.Api.Services;

namespace Realworld.Api.Controllers {

    public record TagEnvelope(string[] Tags);
    public class TagController {
        private readonly ITagService _tagService;
        public TagController(ITagService tagService) {
            _tagService = tagService;
        }

        [HttpGet("api/tags")]
        [ProducesResponseType(type: typeof(TagEnvelope), statusCode: 200)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<TagEnvelope> GetTagsAsync() {
            var tags = await _tagService.GetTagsAsync();
            return new TagEnvelope(tags);
        }
    }

}