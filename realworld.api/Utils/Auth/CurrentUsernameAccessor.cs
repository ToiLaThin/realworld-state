using System.Security.Claims;

namespace Realworld.Api.Utils
{
    public class CurrentUsernameAccessor : ICurrentUsernameAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUsernameAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? GetCurrentUsername()
        {
            return _httpContextAccessor
            .HttpContext?
            .User?
            .Claims?
            .FirstOrDefault(cl => cl.Type == ClaimTypes.NameIdentifier)?
            .Value;
        }
    }
}