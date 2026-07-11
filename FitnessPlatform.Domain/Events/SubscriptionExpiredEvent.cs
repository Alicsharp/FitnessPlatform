using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessPlatform.Domain.Events
{
    // این رویداد بعداً توسط RabbitMQ به سرویس ارسال ایمیل فرستاده می‌شود
    public record SubscriptionExpiredEvent(Guid UserId, Guid SubscriptionId)
    {
        public DateTime ExpiredAt { get; init; } = DateTime.UtcNow;
    }
}
