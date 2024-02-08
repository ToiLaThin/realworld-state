using Microsoft.AspNetCore.Authorization;

namespace Realworld.Api.Utils.Auth
{
    public static class Policy {
        public const string OptionalAuthenticated = "OptionalAuthenticated";
    }

    public class OptionalAuthRequirement : IAuthorizationRequirement
    {
        public OptionalAuthRequirement() { }
    }
}