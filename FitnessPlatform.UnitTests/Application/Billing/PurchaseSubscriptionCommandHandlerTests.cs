using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;
using FitnessPlatform.Application.Features.Billing.Commands.Purchase;
using FitnessPlatform.Domain.Billing;
using FitnessPlatform.Domain.Repositories;

namespace FitnessPlatform.UnitTests.Application.Billing
{
    public class PurchaseSubscriptionCommandHandlerTests
    {
        private readonly Mock<IUserSubscriptionRepository> _subscriptionRepoMock;
        private readonly Mock<ISubscriptionPlanRepository> _planRepoMock;
        private readonly PurchaseSubscriptionCommandHandler _handler;

        public PurchaseSubscriptionCommandHandlerTests()
        {
            _subscriptionRepoMock = new Mock<IUserSubscriptionRepository>();
            _planRepoMock = new Mock<ISubscriptionPlanRepository>();

            _handler = new PurchaseSubscriptionCommandHandler(
                _subscriptionRepoMock.Object,
                _planRepoMock.Object);
        }

        [Fact]
        public async Task Handle_WhenPlanIsInactive_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var plan = SubscriptionPlan.Create("غیرفعال", 1000, 10, 30);
            plan.Deactivate();

            _planRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(plan);

            var command = new PurchaseSubscriptionCommand(Guid.NewGuid(), plan.Id, 1000);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                     .WithMessage("طرح انتخابی معتبر نیست یا غیرفعال شده است.");
        }

        [Fact]
        public async Task Handle_WhenAmountPaidIsLessThanPrice_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var plan = SubscriptionPlan.Create("ویژه", 500000, 10, 30);

            _planRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(plan);

            var command = new PurchaseSubscriptionCommand(Guid.NewGuid(), plan.Id, 400000);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                     .WithMessage("مبلغ پرداختی کمتر از قیمت طرح است.");
        }

        [Fact]
        public async Task Handle_WhenValid_ShouldCreateSubscriptionAndSaveToDatabase()
        {
            // Arrange
            var plan = SubscriptionPlan.Create("ویژه", 500000, 10, 30);
            var userId = Guid.NewGuid();

            _planRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(plan);

            var command = new PurchaseSubscriptionCommand(userId, plan.Id, 500000);

            // Act
            var subscriptionId = await _handler.Handle(command, CancellationToken.None);

            // Assert
            subscriptionId.Should().NotBeEmpty();

            _subscriptionRepoMock.Verify(r => r.AddAsync(
                It.Is<UserSubscription>(s => s.UserId == userId && s.AmountPaid == 500000),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}