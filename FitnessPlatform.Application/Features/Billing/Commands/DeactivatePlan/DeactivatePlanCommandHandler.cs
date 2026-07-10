using FitnessPlatform.Domain.Repositories;
using MediatR;

namespace FitnessPlatform.Application.Features.Billing.Commands.DeactivatePlan
{
    public class DeactivatePlanCommandHandler : IRequestHandler<DeactivatePlanCommand, bool>
    {
        private readonly ISubscriptionPlanRepository _repository;

        public DeactivatePlanCommandHandler(ISubscriptionPlanRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeactivatePlanCommand request, CancellationToken cancellationToken)
        {
            var plan = await _repository.GetByIdAsync(request.PlanId, cancellationToken);

            if (plan == null)
                throw new ArgumentException("طرح مورد نظر یافت نشد.");

            // اجرای منطق بیزینسی لایه دامین
            plan.Deactivate();

            await _repository.UpdateAsync(plan, cancellationToken);

            return true;
        }
    }
}
