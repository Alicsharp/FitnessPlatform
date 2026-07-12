namespace FitnessPlatform.Domain.Common
{
    public class OutboxMessage
    {
        public Guid Id { get; private set; }
        public string Type { get; private set; } // نام یا نوع رویداد (مثلاً WorkoutSessionCompletedEvent)
        public string Content { get; private set; } // دیتای رویداد که به صورت JSON ذخیره می‌شود
        public DateTime OccurredOn { get; private set; } // زمان وقوع رویداد
        public DateTime? ProcessedOn { get; private set; } // زمان ارسال موفق به RabbitMQ
        public string? Error { get; private set; } // خطاهای احتمالی در زمان ارسال

        private OutboxMessage() { } // برای EF Core

        public static OutboxMessage Create(string type, string content)
        {
            return new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = type,
                Content = content,
                OccurredOn = DateTime.UtcNow
            };
        }

        public void MarkAsProcessed()
        {
            ProcessedOn = DateTime.UtcNow;
        }

        public void LogError(string error)
        {
            Error = error;
        }
    }
}
