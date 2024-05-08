namespace Sos.Application.Core.Abstractions.Cache
{
    /// <summary>
    /// Represents a cache service interface.
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Set cache with asynchronous.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="expiredAt">The expired at.</param>
        /// <returns></returns>
        Task SetAsync(string key, object value, TimeSpan? expiredAt);

        /// <summary>
        /// Get cache with asynchronous.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value type of string.</returns>
        Task<string> GetAsync(string key);

        /// <summary>
        /// Remove cache with asynchronous
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <returns></returns>
        Task RemoveAsync(string pattern);
    }
}
