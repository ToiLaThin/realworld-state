using Microsoft.AspNetCore.Authorization;

namespace Realworld.Api.Utils.Auth {
    public class OptionalAuthHandler: AuthorizationHandler<OptionalAuthRequirement>
    {
        public OptionalAuthHandler()
        {
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OptionalAuthRequirement requirement)
        {
            //just mark authorization as succeed
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}