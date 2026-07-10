using FitnessPlatform.Domain.Common;

namespace FitnessPlatform.Domain.Billing
{
    public class SubscriptionPlan : Entity
    {
        public string Title { get; private set; }
        public decimal Price { get; private set; }
        public int TotalSessions { get; private set; }
        public int ValidityDays { get; private set; }
        public bool IsActive { get; private set; }

        // سازنده خالی و پرایوت فقط برای استفاده EF Core
        private SubscriptionPlan()
        {
            Title = string.Empty;
        }

        // متد Factory: تنها راه اصولی برای ساخت یک طرح اشتراک جدید
        public static SubscriptionPlan Create(string title, decimal price, int totalSessions, int validityDays)
        {
            if (price < 0)
                throw new ArgumentException("قیمت نمی‌تواند منفی باشد.");
            if (totalSessions <= 0)
                throw new ArgumentException("تعداد جلسات باید بیشتر از صفر باشد.");
            if (validityDays <= 0)
                throw new ArgumentException("مدت اعتبار باید بیشتر از صفر روز باشد.");

            var plan = new SubscriptionPlan
            {
                Id = Guid.NewGuid(),
                Title = title,
                Price = price,
                TotalSessions = totalSessions,
                ValidityDays = validityDays,
                IsActive = true
            };

            // ثبت رویداد دامین
            plan.AddDomainEvent(new { EventName = "SubscriptionPlanCreated", PlanId = plan.Id });

            return plan;
        }

        // ==========================================
        // رفتارها (Behaviors)
        // ==========================================

        public void Deactivate()
        {
            if (!IsActive)
                throw new InvalidOperationException("این طرح قبلاً غیرفعال شده است.");

            IsActive = false;
            UpdateTimestamp();
        }

        public void Activate()
        {
            if (IsActive)
                throw new InvalidOperationException("این طرح در حال حاضر فعال است.");

            IsActive = true;
            UpdateTimestamp();
        }
    }
}
