using FitnessPlatform.Infrastructure.Persistence.FitnessPlatform.Infrastructure.Persistence;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FitnessPlatform.Infrastructure.BackgroundJobs
{
    // جلوگیری از اجرای همزمان دو نمونه از این Job
    [DisallowConcurrentExecution]
    public class OutboxMessageProcessorJob : IJob
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<OutboxMessageProcessorJob> _logger;

        public OutboxMessageProcessorJob(
            ApplicationDbContext dbContext,
            IPublishEndpoint publishEndpoint,
            ILogger<OutboxMessageProcessorJob> logger)
        {
            _dbContext = dbContext;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            // خواندن ۲۰ پیام پردازش نشده
            var messages = await _dbContext.OutboxMessages
                .Where(m => m.ProcessedOn == null)
                .OrderBy(m => m.OccurredOn)
                .Take(20)
                .ToListAsync(context.CancellationToken);

            if (!messages.Any()) return;

            foreach (var message in messages)
            {
                try
                {
                    // بازیابی نوع اصلی رویداد
                    var eventType = Type.GetType(message.Type);
                    if (eventType == null) continue;

                    // تبدیل JSON به شیء واقعی
                    var domainEvent = JsonConvert.DeserializeObject(message.Content, eventType, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });

                    if (domainEvent == null) continue;

                    // ارسال به RabbitMQ از طریق MassTransit
                    await _publishEndpoint.Publish(domainEvent, eventType, context.CancellationToken);

                    // علامت‌گذاری به عنوان ارسال شده
                    message.MarkAsProcessed();
                    _logger.LogInformation($"پیام Outbox با آیدی {message.Id} با موفقیت به RabbitMQ ارسال شد.");
                }
                catch (Exception ex)
                {
                    message.LogError(ex.Message);
                    _logger.LogError($"خطا در ارسال پیام Outbox با آیدی {message.Id}: {ex.Message}");
                }
            }

            await _dbContext.SaveChangesAsync(context.CancellationToken);
        }
    }
}
