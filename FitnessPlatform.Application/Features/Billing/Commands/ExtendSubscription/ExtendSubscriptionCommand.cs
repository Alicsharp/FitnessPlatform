using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessPlatform.Application.Features.Billing.Commands.ExtendSubscription
{
    public record ExtendSubscriptionCommand(Guid SubscriptionId, int ExtraDays) : IRequest<bool>;
}
