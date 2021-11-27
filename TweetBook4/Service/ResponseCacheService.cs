using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TweetBook4.Service
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDistributedCache _distributedCache;
        public ResponseCacheService(IDistributedCache distributedCache)
        {
            this._distributedCache = distributedCache;
        }
        public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeTimeLive)
        {
            if (response == null)
            {
                return;
            }
            var serilaizeResponse = JsonConvert.SerializeObject(response);
            await _distributedCache.SetStringAsync(cacheKey, serilaizeResponse, new DistributedCacheEntryOptions 
            { 
                AbsoluteExpirationRelativeToNow=timeTimeLive
            });
        }

        public async Task<string> GetCacheResponseAsync(string cacheKe)
        {
            var cachedResponse = await _distributedCache.GetStringAsync(cacheKe);
            return string.IsNullOrEmpty(cachedResponse) ? null : cachedResponse;
        }
    }
}
