using FitnessPlatform.Application.Events;
using MassTransit;
using MediatR;

namespace FitnessPlatform.Application.Features.WorkoutSessions.Commands.CompleteSession
{
    public class CompleteWorkoutSessionCommandHandler : IRequestHandler<CompleteWorkoutSessionCommand, bool>
    {
        // ابزار پرتاب پیام به RabbitMQ
        private readonly IPublishEndpoint _publishEndpoint;

        // در یک پروژه واقعی، اینجا Repository یا DbContext را هم تزریق می‌کنی
        // private readonly IWorkoutSessionRepository _repository;

        public CompleteWorkoutSessionCommandHandler(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task<bool> Handle(CompleteWorkoutSessionCommand request, CancellationToken cancellationToken)
        {
            // --- بخش اول: آپدیت دیتابیس (منطق DDD) ---
            // ۱. جلسه تمرین را از دیتابیس پیدا می‌کنیم
            // var session = await _repository.GetByIdAsync(request.SessionId);

            // ۲. متد دامین را صدا می‌زنیم تا وضعیت تکمیل تغییر کند
            // session.MarkAsCompleted();

            // ۳. ذخیره در دیتابیس
            // await _repository.SaveChangesAsync();

            // --- بخش دوم: پرتاب رویداد به RabbitMQ ---
            // حالا که تمرین مثلاً یک ساعت سایه‌زنی و کیسه زدن تمام شده و در دیتابیس نشست، رویداد را می‌سازیم
            var sessionCompletedEvent = new WorkoutSessionCompletedEvent(
                SessionId: request.SessionId,
                Title: "تمرینات ترکیبی هوازی و بوکس", // در دنیای واقعی این را از دیتابیس می‌خوانیم
                BurnedCalories: 650, // این هم از دیتابیس می‌آید
                CompletedAt: DateTime.UtcNow
            );

            // شلیک پیام به صف! 
            // بلافاصله بعد از اجرای این خط، هر دو کلاس Consumer که ساختی در پس‌زمینه بیدار می‌شوند
            await _publishEndpoint.Publish(sessionCompletedEvent, cancellationToken);

            // برگرداندن جواب موفقیت‌آمیز به کنترلر (کاربر معطل نمی‌ماند)
            return true;
        }
    }
}
