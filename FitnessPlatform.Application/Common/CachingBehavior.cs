using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace FitnessPlatform.Application.Common
{
    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<CachingBehavior<TRequest, TResponse>> _logger;

        public CachingBehavior(IDistributedCache cache, ILogger<CachingBehavior<TRequest, TResponse>> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // اگر کوئری ما قابلیت کش شدن نداشت، مستقیم برو سراغ هندلر و دیتابیس
            if (request is not ICacheableQuery cacheableQuery)
            {
                return await next();
            }

            var cacheKey = cacheableQuery.CacheKey;

            // ۱. بررسی وجود دیتا در Redis
            var cachedData = await _cache.GetAsync(cacheKey, cancellationToken);
            if (cachedData != null)
            {
                _logger.LogInformation("✅ داده‌ها از Redis برای کلید {CacheKey} خوانده شد.", cacheKey);
                var json = Encoding.UTF8.GetString(cachedData);
                return JsonSerializer.Deserialize<TResponse>(json);
            }

            // ۲. اگر در کش نبود، هندلر اصلی را اجرا کن (رفتن به سمت SQL)
            _logger.LogInformation("⚠️ کش خالی بود. واکشی داده‌ها از دیتابیس برای کلید {CacheKey}...", cacheKey);
            var response = await next();

            // ۳. ذخیره نتیجه در Redis برای درخواست‌های بعدی
            var serializedData = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response));
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = cacheableQuery.Expiration ?? TimeSpan.FromMinutes(10)
            };

            await _cache.SetAsync(cacheKey, serializedData, options, cancellationToken);
            _logger.LogInformation("💾 داده‌ها در Redis ذخیره شدند.");

            return response;
        }
    }
}
