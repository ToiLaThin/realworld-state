namespace Realworld.Api.Utils.ExceptionHandling
{
    public static class ConduitErrors {
        public const string NOT_FOUND = "not found";
        public const string IN_USE = "in use";

        public const string UNAUTHORIZED = "do not have permission to perform action";
        public const string InternalServerError = nameof(InternalServerError);
    }
}