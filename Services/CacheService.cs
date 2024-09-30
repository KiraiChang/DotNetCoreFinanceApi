using FinanceApi.Interfaces.Services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FinanceApi.Services
{
    /// <summary>
    /// cache service
    /// </summary>
    public class CacheService : ICacheService
    {
        /// <summary>
        /// fail message template
        /// </summary>
        private const string FailsMessageTemplate = "{methodName} fails";

        /// <summary>
        /// lock
        /// </summary>
        private static readonly ConcurrentDictionary<string, SemaphoreSlim> _locks = new();

        /// <summary>
        /// cache
        /// </summary>
        private readonly IDistributedCache _distributedCache = null;

        /// <summary>
        /// logger
        /// </summary>
        protected readonly ILogger _logger = null;

        /// <summary>
        /// .actor
        /// </summary>
        /// <param name="distributedCache"></param>
        /// <param name="logger"></param>
        public CacheService(IDistributedCache distributedCache,
            ILogger<CacheService> logger)
        {
            _distributedCache = distributedCache;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<T> GetAsync<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return default;
            }

            try
            {
                var bytes = await _distributedCache.GetAsync(key);
                return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(bytes));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: FailsMessageTemplate, nameof(GetAsync));
            }

            return default;
        }

        /// <inheritdoc/>
        public async Task<bool> SetAsync<T>(string key, T obj, DateTime expired)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }

            if (obj == null)
            {
                return true;
            }

            var str = JsonConvert.SerializeObject(obj);
            var bytes = Encoding.UTF8.GetBytes(str);
            await _distributedCache.SetAsync(key, bytes, options: new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = expired
            });
            return await Task.FromResult(true);
        }

        /// <inheritdoc/>
        public async Task<T> GetOrAddAsync<T>(string funcName, string key, Func<Task<T>> invoke, DateTime expireTime)
        {
            if (string.IsNullOrEmpty(key))
            {
                return default;
            }
            T result = default;
            var locker = _locks.GetOrAdd<SemaphoreSlim>(funcName, (keyValue, old) => old, new SemaphoreSlim(1, 1));
            try
            {
                await locker.WaitAsync();
                var query = await GetAsync<T>(key);
                if (query != null)
                {
                    result = query;
                }
                else if (invoke != null)
                {
                    T quertResult = await invoke();
                    if (quertResult != null)
                    {
                        await SetAsync(key, quertResult, expireTime);
                        result = quertResult;
                    }
                }
            }
            finally
            {
                locker.Release();
            }

            return result;
        }
    }
}
