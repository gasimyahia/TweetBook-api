using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetBook4.Service;

namespace TweetBook4.Cache
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveSeconds;
        public CachedAttribute(int timeToLiveSeconds)
        {
            this._timeToLiveSeconds = timeToLiveSeconds;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // before controller
            // check if the request is cached if true return
            var cacheSetting = context.HttpContext.RequestServices.GetRequiredService<RedisCacheSettings>();
            if (!cacheSetting.Enabled)
            {
                await next();
                return;
            }
            var cacheSerivce = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
            var cachedResponse = await cacheSerivce.GetCacheResponseAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedResponse))
            {
                var contentResult = new ContentResult
                {
                    Content=cachedResponse,
                    ContentType="application/json",
                    StatusCode=200
                };
                context.Result = contentResult;
                return;
            }
            // when the cache not exists
            var exeutedContex= await next();
            if (exeutedContex.Result is OkObjectResult objectResult) 
            {
                await cacheSerivce.CacheResponseAsync(cacheKey, objectResult.Value, TimeSpan.FromSeconds(_timeToLiveSeconds));
            };
            
        }

        private static string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append($"{request.Path}");
            foreach(var (key,value)in request.Query.OrderBy(x=> x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }
            return keyBuilder.ToString();
        }
    }
}
