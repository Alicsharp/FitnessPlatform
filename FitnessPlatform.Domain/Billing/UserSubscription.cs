using FitnessPlatform.Domain.Common;
using FitnessPlatform.Domain.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessPlatform.Domain.Billing
{
    public class UserSubscription : Entity // اضافه شدن ارث‌بری برای یکپارچگی
    {
        public Guid UserId { get; private set; }
        public Guid SubscriptionPlanId { get; private set; }

        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        public int TotalSessions { get; private set; }
        public int RemainingSessions { get; private set; }
        public decimal AmountPaid { get; private set; }

        // قفل هوشمند (Smart Lock)
        public bool IsValid => DateTime.UtcNow <= EndDate && RemainingSessions > 0;

        // سازنده پرایوت برای EF Core
        private UserSubscription() { }

        // سازنده اصلی هم پرایوت شد تا کسی نتواند از بیرون مستقیم new کند
        private UserSubscription(Guid id, Guid userId, Guid planId, int totalSessions, int validityDays, decimal amountPaid) : base(id)
        {
            UserId = userId;
            SubscriptionPlanId = planId;
            TotalSessions = totalSessions;
            RemainingSessions = totalSessions;
            AmountPaid = amountPaid;

            StartDate = DateTime.UtcNow;
            EndDate = StartDate.AddDays(validityDays);
        }

        // الگوی Factory Method برای ساخت یک خرید جدید
        public static UserSubscription Create(Guid userId, Guid planId, int totalSessions, int validityDays, decimal amountPaid)
        {
            var subscription = new UserSubscription(Guid.NewGuid(), userId, planId, totalSessions, validityDays, amountPaid);

            // پرتاب رویداد بیزینسی (مثلاً برای اینکه بعداً MediatR آن را بگیرد و در صف RabbitMQ بیندازد تا ایمیل فاکتور ارسال شود)
            subscription.AddDomainEvent(new { EventName = "UserSubscriptionPurchased", UserId = userId, PlanId = planId, Amount = amountPaid });

            return subscription;
        }

        // متد کسر جلسه (شمارنده)
        public void UseSession()
        {
            if (!IsValid)
            {
                throw new InvalidOperationException("اشتراک شما منقضی شده یا جلسات شما به پایان رسیده است. امکان ثبت تمرین وجود ندارد.");
            }

            RemainingSessions--;
            UpdateTimestamp(); // ⚡️ اضافه شدن ثبت زمانِ آپدیت

            // در صورت نیاز به ارسال نوتیفیکیشن برای اتمام جلسات
            if (RemainingSessions == 0)
            {
                AddDomainEvent(new { EventName = "UserSubscriptionFinished", UserId = UserId });
            }
        }
        public void Expire()
        {
            // اگر از قبل منقضی شده، کاری نمی‌کنیم
            if (RemainingSessions == 0 && DateTime.UtcNow > EndDate) return;

            RemainingSessions = 0; // صفر کردن جلسات
            EndDate = DateTime.UtcNow.AddDays(-1); // تنظیم تاریخ به گذشته برای اطمینان از نامعتبر شدن

            UpdateTimestamp();

            // ⚡️ پرتاب رویداد دامین برای RabbitMQ
            AddDomainEvent(new SubscriptionExpiredEvent(UserId, Id));
        }
        // متد تمدید دستی
        public void ExtendEndDate(int extraDays)
        {
            EndDate = EndDate.AddDays(extraDays);
            UpdateTimestamp(); // ⚡️ اضافه شدن ثبت زمانِ آپدیت
        }
    }
}
