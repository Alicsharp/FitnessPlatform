using FitnessPlatform.Application.Features.Billing.Commands.DeductSession;
using FitnessPlatform.Domain.Billing;
using FitnessPlatform.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace FitnessPlatform.UnitTests.Application.Billing
{
    public class DeductSubscriptionSessionCommandHandlerTests
    {
        private readonly Mock<IUserSubscriptionRepository> _repositoryMock;
        private readonly DeductSubscriptionSessionCommandHandler _handler;

        public DeductSubscriptionSessionCommandHandlerTests()
        {
            _repositoryMock = new Mock<IUserSubscriptionRepository>();
            _handler = new DeductSubscriptionSessionCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_WhenNoActiveSubscriptionFound_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var command = new DeductSubscriptionSessionCommand(Guid.NewGuid());

            _repositoryMock.Setup(r => r.GetActiveSubscriptionByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync((UserSubscription)null!);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                     .WithMessage("شما هیچ اشتراک فعالی برای کسر جلسه ندارید.");
        }

        [Fact]
        public async Task Handle_WhenActiveSubscriptionExists_ShouldDeductSessionAndUpdateDatabase()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new DeductSubscriptionSessionCommand(userId);

            var subscription = UserSubscription.Create(userId, Guid.NewGuid(), 10, 30, 1000);

            _repositoryMock.Setup(r => r.GetActiveSubscriptionByUserIdAsync(userId, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(subscription);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            subscription.RemainingSessions.Should().Be(9);

            _repositoryMock.Verify(r => r.UpdateAsync(subscription, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
