using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Sos.Application.Core.Abstractions.Cache;
using System.Text;

namespace Sos.Api.Attributes
{
    /// <summary>
    /// Represents the cache attribute.
    /// </summary>
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveSeconds;

        /// <summary>
        /// Intializes a new instance of the <see cref="CacheAttribute"/> class.
        /// </summary>
        /// <param name="timeToLiveSeconds">The time to live in seconds of the cache.</param>
        public CacheAttribute(int timeToLiveSeconds = 300)
        {
            _timeToLiveSeconds = timeToLiveSeconds;
        }

        // < inheritdoc />
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();

            if (cacheService == null)
            {
                await next();
                return;
            }

            var cacheKey = GenerateCacheKey(context.HttpContext.Request);

            Console.WriteLine($"===> CacheKey: {cacheKey}");

            var cacheValue = await cacheService.GetAsync(cacheKey);

            if (!string.IsNullOrEmpty(cacheValue))
            {
                var contentResult = new ContentResult()
                {
                    Content = cacheValue,
                    ContentType = "application/json; charset=utf-8",
                    StatusCode = StatusCodes.Status200OK,
                };

                context.Result = contentResult;

                return;
            }

            var executedContext = await next();

            if (executedContext.Result is OkObjectResult okObjectResult && okObjectResult.Value != null)
            {
                await cacheService.SetAsync(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(_timeToLiveSeconds));
            }
        }

        private static string GenerateCacheKey(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();

            keyBuilder.Append($"{request.Path}?");

            var requestQueries = request.Query.OrderBy(item => item.Key);

            foreach (var (key, value) in requestQueries)
            {
                keyBuilder.Append($"{key}={value}&");
            }

            keyBuilder.Remove(keyBuilder.Length - 1, 1);

            return keyBuilder.ToString();
        }
    }
}
