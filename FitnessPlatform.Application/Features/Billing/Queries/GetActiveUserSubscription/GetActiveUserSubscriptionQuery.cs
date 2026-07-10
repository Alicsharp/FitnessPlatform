using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessPlatform.Application.Features.Billing.Queries.GetActiveUserSubscription
{
    // ۲. درخواستِ کوئری با گرفتن شناسه کاربر
    public record GetActiveUserSubscriptionQuery(Guid UserId) : IRequest<UserSubscriptionDto?>;
}
