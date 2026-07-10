using FitnessPlatform.Application.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessPlatform.Infrastructure.Messaging.Consumers
{
    public class SendCongratulationEmailConsumer : IConsumer<WorkoutSessionCompletedEvent>
    {
        private readonly ILogger<SendCongratulationEmailConsumer> _logger;

        // تزریق وابستگی برای لاگر (یا سرویس ایمیل واقعی در آینده)
        public SendCongratulationEmailConsumer(ILogger<SendCongratulationEmailConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<WorkoutSessionCompletedEvent> context)
        {
            // استخراج اطلاعات از پیامی که از صف RabbitMQ آمده است
            var sessionTitle = context.Message.Title;
            var calories = context.Message.BurnedCalories;

            // در اینجا به جای ارسال واقعی ایمیل، فعلاً یک لاگ چاپ می‌کنیم
            _logger.LogInformation($"[RabbitMQ Worker] ایمیل تبریک ارسال شد! تمرین: {sessionTitle} - کالری سوزانده شده: {calories}");

            return Task.CompletedTask;
        }
    }

}
