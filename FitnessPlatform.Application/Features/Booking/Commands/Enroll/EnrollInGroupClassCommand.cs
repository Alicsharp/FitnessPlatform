using FitnessPlatform.Application.Exceptions;
using FitnessPlatform.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessPlatform.Application.Features.Booking.Commands.Enroll
{
    public record EnrollInGroupClassCommand(Guid UserId, Guid GroupClassId) : IRequest<bool>;

    public class EnrollInGroupClassCommandHandler : IRequestHandler<EnrollInGroupClassCommand, bool>
    {
        private readonly IGroupClassRepository _repository;

        public EnrollInGroupClassCommandHandler(IGroupClassRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(EnrollInGroupClassCommand request, CancellationToken cancellationToken)
        {
            var groupClass = await _repository.GetByIdAsync(request.GroupClassId, cancellationToken);

            if (groupClass == null)
                throw new ArgumentException("کلاس مورد نظر یافت نشد.");

            // ۱. صدا زدن منطق دامین (اگر ظرفیت در حافظه رم پر باشد، همینجا خطا می‌دهد)
            groupClass.EnrollMember(request.UserId);

            try
            {
                // ۲. تلاش برای ثبت در دیتابیس
                await _repository.UpdateAsync(groupClass, cancellationToken);
                return true;
            }
            catch (ConcurrencyException) // ⚡️ شکار خطای بیزینسی، بدون هیچ وابستگی به EF Core
            {
                // ۳. ⚡️ مدیریت تمیز خطای همزمانی (Race Condition)
                throw new InvalidOperationException("متأسفانه شخص دیگری در همین لحظه آخرین ظرفیت کلاس را رزرو کرد. لطفاً کلاس دیگری را انتخاب کنید.");
            }
        }
    }
}
