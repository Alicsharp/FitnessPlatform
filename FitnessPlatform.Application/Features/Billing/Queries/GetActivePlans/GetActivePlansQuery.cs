using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessPlatform.Application.Features.Billing.Queries.GetActivePlans
{
    public record GetActivePlansQuery() : IRequest<IEnumerable<SubscriptionPlanDto>>;
}
