using System;
using System.Threading.Tasks;

namespace FinanceApi.Interfaces.Services
{
    /// <summary>
    /// Cache service
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// get cache
        /// </summary>
        /// <typeparam name="T">type of T</typeparam>
        /// <param name="key">cache key</param>
        /// <returns>task of cache result</returns>
        Task<T> GetAsync<T>(string key);

        /// <summary>
        /// set cache
        /// </summary>
        /// <typeparam name="T">type of T</typeparam>
        /// <param name="key">cache key</param>
        /// <param name="obj">cache value</param>
        /// <param name="expired">expired time</param>
        /// <returns>task of insert result</returns>
        Task<bool> SetAsync<T>(string key, T obj, DateTime expired);

        /// <summary>
        /// GetOrAdd
        /// </summary>
        /// <typeparam name="T">type of T</typeparam>
        /// <param name="funcName">function name</param>
        /// <param name="key">cache key</param>
        /// <param name="invoke">no data invoke func</param>
        /// <param name="expireTime">expired time</param>
        /// <returns>task of cache result</returns>
        Task<T> GetOrAddAsync<T>(string funcName, string key, Func<Task<T>> invoke, DateTime expireTime);
    }
}
