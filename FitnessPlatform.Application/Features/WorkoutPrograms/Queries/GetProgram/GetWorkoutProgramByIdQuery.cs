using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace FitnessPlatform.Application.Features.WorkoutPrograms.Queries.GetProgram
{
    // دستور خواندن اطلاعات
    public record GetWorkoutProgramByIdQuery(Guid Id) : IRequest<string>; // برای سادگی فرض می‌کنیم خروجی یک متن JSON است

    public class GetWorkoutProgramByIdQueryHandler : IRequestHandler<GetWorkoutProgramByIdQuery, string>
    {
        // تزریق اینترفیس کش (بدون وابستگی به نام Redis)
        private readonly IDistributedCache _cache;
        // private readonly IWorkoutProgramRepository _repository;

        public GetWorkoutProgramByIdQueryHandler(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<string> Handle(GetWorkoutProgramByIdQuery request, CancellationToken cancellationToken)
        {
            // ۱. ساخت یک کلید یکتا برای این دیتای خاص
            string cacheKey = $"WorkoutProgram_{request.Id}";

            // ۲. تلاش برای خواندن از روی حافظه پرسرعت Redis
            var cachedData = await _cache.GetStringAsync(cacheKey, cancellationToken);

            if (!string.IsNullOrEmpty(cachedData))
            {
                // اگر دیتا در ردیس بود، با سرعت نور برش می‌گردانیم!
                return $"[خوانده شده از Redis] - {cachedData}";
            }

            // ۳. اگر دیتا در ردیس نبود (Cache Miss)، تازه می‌رویم سراغ SQL Server
            // var program = await _repository.GetByIdAsync(request.Id);
            var fakeProgramFromDb = new { Id = request.Id, Title = "برنامه فشرده کاهش وزن", IsActive = true };

            // ۴. دیتا را به فرمت JSON تبدیل می‌کنیم
            string serializedData = JsonSerializer.Serialize(fakeProgramFromDb);

            // ۵. دیتا را برای درخواست‌های بعدی در Redis ذخیره می‌کنیم (با تاریخ انقضای ۱۰ دقیقه‌ای)
            var cacheOptions = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

            await _cache.SetStringAsync(cacheKey, serializedData, cacheOptions, cancellationToken);

            // ۶. برگرداندن دیتای خوانده شده از دیتابیس
            return $"[خوانده شده از SQL Server و ذخیره در کش] - {serializedData}";
        }
    }
}
