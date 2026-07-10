using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessPlatform.Application.Features.Billing.Commands.CreatePlan
{
    // ریکورد برای دریافت اطلاعات طرح جدید
    public record CreateSubscriptionPlanCommand(
        string Title,
        decimal Price,
        int TotalSessions,
        int ValidityDays) : IRequest<Guid>;
}

