using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessPlatform.Application.Features.Billing.Commands.DeactivatePlan
{
    public record DeactivatePlanCommand(Guid PlanId) : IRequest<bool>;
}
