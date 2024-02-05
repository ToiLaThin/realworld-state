namespace Realworld.Api.Utils
{
    /// <summary>
    /// Interface for getting the current username from http context claims. Used when user logged in
    /// </summary>
    public interface ICurrentUsernameAccessor
    {
        public string? GetCurrentUsername();
    }
}