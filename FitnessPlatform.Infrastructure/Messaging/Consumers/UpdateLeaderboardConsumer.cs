using FitnessPlatform.Application.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessPlatform.Infrastructure.Messaging.Consumers
{
    public class UpdateLeaderboardConsumer : IConsumer<WorkoutSessionCompletedEvent>
    {
        private readonly ILogger<UpdateLeaderboardConsumer> _logger;

        public UpdateLeaderboardConsumer(ILogger<UpdateLeaderboardConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<WorkoutSessionCompletedEvent> context)
        {
            var sessionId = context.Message.SessionId;
            var calories = context.Message.BurnedCalories;

            // در اینجا منطق محاسبه امتیاز و ارتقای رتبه کاربر در جدول رقابت‌ها قرار می‌گیرد
            // فعلاً برای تست، یک لاگ موفقیت چاپ می‌کنیم
            _logger.LogInformation($"[RabbitMQ Worker] جدول امتیازات آپدیت شد! شناسه تمرین: {sessionId} - امتیاز اضافه شده: {calories}");

            return Task.CompletedTask;
        }
    }
}
