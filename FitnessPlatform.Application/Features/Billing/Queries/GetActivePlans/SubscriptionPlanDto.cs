using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessPlatform.Application.Features.Billing.Queries.GetActivePlans
{
    // ۱. ساخت DTO برای کپسوله‌سازی و انتقال دیتای تمیز به فرانت‌اند
    public record SubscriptionPlanDto(
        Guid Id,
        string Title,
        decimal Price,
        int TotalSessions,
        int ValidityDays);
}
