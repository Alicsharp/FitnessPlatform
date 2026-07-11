using FitnessPlatform.Application.Features.Billing.Commands;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessPlatform.Infrastructure.BackgroundJobs
{
    // این ویژگی می‌گوید که این جاب نباید همزمان چند بار اجرا شود
    [DisallowConcurrentExecution]
    public class SubscriptionExpirationJob : IJob
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SubscriptionExpirationJob> _logger;

        public SubscriptionExpirationJob(IMediator mediator, ILogger<SubscriptionExpirationJob> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("⏳ Job شروع شد: در حال بررسی اشتراک‌های منقضی شده در زمان {Time}", DateTime.UtcNow);

            try
            {
                // ⚡️ شلیک کامند به سمت لایه Application
                var command = new ProcessExpiredSubscriptionsCommand();
                var count = await _mediator.Send(command);

                _logger.LogInformation("✅ Job با موفقیت پایان یافت. تعداد {Count} اشتراک منقضی و به RabbitMQ ارسال شد.", count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ خطایی در هنگام پردازش اشتراک‌های منقضی رخ داد.");

                // در صورت نیاز به تلاش مجدد (Retry) در Quartz:
                throw new JobExecutionException(ex, refireImmediately: true);
            }
        }
    }
}
