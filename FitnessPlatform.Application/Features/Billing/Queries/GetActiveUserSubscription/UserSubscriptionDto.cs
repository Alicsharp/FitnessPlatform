using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessPlatform.Application.Features.Billing.Queries.GetActiveUserSubscription
{
    // ۱. خروجیِ تمیز برای پروفایل کاربر
    public record UserSubscriptionDto(
        Guid Id,
        Guid PlanId,
        DateTime StartDate,
        DateTime EndDate,
        int TotalSessions,
        int RemainingSessions,
        bool IsValid); // این همان قفل هوشمند است که به سمت کلاینت می‌فرستیم

}
