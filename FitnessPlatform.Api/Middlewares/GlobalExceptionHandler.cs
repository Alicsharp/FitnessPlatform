using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace FitnessPlatform.Api.Middlewares
{
    // اینترفیس IExceptionHandler جدیدترین روش دات‌نت برای مدیریت خطاهاست
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            // ۱. لاگ کردن خطای رخ داده با استفاده از Serilog که قبلاً تنظیم کردیم
            _logger.LogError(exception, "خطای سیستمی در مسیر {Path} رخ داد: {Message}",
                httpContext.Request.Path, exception.Message);

            // ۲. ساختار استاندارد RFC 7807 برای پاسخ به کلاینت (فرانت‌اند / موبایل)
            var problemDetails = new ProblemDetails
            {
                Instance = httpContext.Request.Path
            };

            // ۳. دسته‌بندی خطاها (Mapping)
            // ما در لایه Domain برای خطاهای بیزینسی (مثل پر بودن ظرفیت) از InvalidOperationException استفاده کردیم
            if (exception is InvalidOperationException || exception is ArgumentException)
            {
                problemDetails.Title = "خطای قوانین کسب‌وکار (Business Rule Violation)";
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Detail = exception.Message; // نمایش پیام خطای دامین به کاربر
                problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
            }
            // اگر خطای ناشناخته‌ای بود (مثلاً قطع شدن دیتابیس)
            else
            {
                problemDetails.Title = "خطای داخلی سرور (Internal Server Error)";
                problemDetails.Status = StatusCodes.Status500InternalServerError;
                problemDetails.Detail = "یک خطای غیرمنتظره در سرور رخ داده است. لطفاً با پشتیبانی تماس بگیرید.";
                problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";
            }

            // ۴. اعمال کد وضعیت (Status Code) روی خروجی HTTP
            httpContext.Response.StatusCode = problemDetails.Status.Value;

            // ۵. ارسال JSON استاندارد به کلاینت
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            // برگرداندن true به این معنی است که ما خطا را مدیریت کردیم و نیاز به کار بیشتری نیست
            return true;
        }
    }
}
