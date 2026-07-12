using FitnessPlatform.Application.Events;
using FitnessPlatform.Application.Features.WorkoutSessions.Commands.CompleteSession;
using FluentAssertions;
using MassTransit;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessPlatform.UnitTests.Application.WorkoutSessions
{
    public class CompleteWorkoutSessionCommandHandlerTests
    {
        private readonly Mock<IPublishEndpoint> _publishEndpointMock;
        private readonly CompleteWorkoutSessionCommandHandler _handler;

        public CompleteWorkoutSessionCommandHandlerTests()
        {
            // ۱. ساخت نسخه قلابی از ابزار ارسال پیام MassTransit
            _publishEndpointMock = new Mock<IPublishEndpoint>();

            // ۲. تزریق آن به هندلر
            _handler = new CompleteWorkoutSessionCommandHandler(_publishEndpointMock.Object);
        }

        [Fact]
        public async Task Handle_WhenCalled_ShouldPublishEventToRabbitMQAndReturnTrue()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var command = new CompleteWorkoutSessionCommand(sessionId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            // بررسی موفقیت‌آمیز بودن خروجی
            result.Should().BeTrue();

            // ⚡️ بررسی طلایی: آیا رویداد با دیتای درست به سمت RabbitMQ شلیک شد؟
            _publishEndpointMock.Verify(p => p.Publish(
                It.Is<WorkoutSessionCompletedEvent>(e =>
                    e.SessionId == sessionId &&
                    e.Title == "تمرینات ترکیبی هوازی و بوکس" &&
                    e.BurnedCalories == 650),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
