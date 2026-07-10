using FitnessPlatform.Domain.Repositories;
using MediatR;

namespace FitnessPlatform.Application.Features.Billing.Commands.ExtendSubscription
{
    public class ExtendSubscriptionCommandHandler : IRequestHandler<ExtendSubscriptionCommand, bool>
    {
        private readonly IUserSubscriptionRepository _repository;

        public ExtendSubscriptionCommandHandler(IUserSubscriptionRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(ExtendSubscriptionCommand request, CancellationToken cancellationToken)
        {
            if (request.ExtraDays <= 0)
                throw new ArgumentException("تعداد روزهای تمدید باید بیشتر از صفر باشد.");

            var subscription = await _repository.GetByIdAsync(request.SubscriptionId, cancellationToken);

            if (subscription == null)
                throw new ArgumentException("اشتراک مورد نظر یافت نشد.");

            // اجرای منطق ارفاقی دامین
            subscription.ExtendEndDate(request.ExtraDays);

            await _repository.UpdateAsync(subscription, cancellationToken);

            return true;
        }
    }
}
