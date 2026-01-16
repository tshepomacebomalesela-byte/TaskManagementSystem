using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TaskApplication.Common.Interfaces;

namespace TaskApplication.Common.Behaviors
{
    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICachableQuery, IRequest<TResponse>
    {
        private readonly IDistributedCache _cache;

        public CachingBehavior(IDistributedCache cache) => _cache = cache;

        /// <summary>
        /// Method to handle the cashing of the tasks
        /// </summary>
        /// <param name="request"></param>
        /// <param name="next"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
        {
            var cachedData = await _cache.GetStringAsync(request.CacheKey, ct);
            if (cachedData != null)
            {
                return JsonSerializer.Deserialize<TResponse>(cachedData)!;
            }

            var response = await next();

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = request.Expiration ?? TimeSpan.FromMinutes(5)
            };

            await _cache.SetStringAsync(request.CacheKey, JsonSerializer.Serialize(response), options, ct);

            return response;
        }
    }
}
