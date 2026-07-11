using FitnessPlatform.Application.Features.Billing.Commands.ExtendSubscription;
using FitnessPlatform.Domain.Billing;
using FitnessPlatform.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace FitnessPlatform.UnitTests.Application.Billing
{
 
    public class ExtendSubscriptionCommandHandlerTests
    {
        private readonly Mock<IUserSubscriptionRepository> _repositoryMock;
        private readonly ExtendSubscriptionCommandHandler _handler;

        public ExtendSubscriptionCommandHandlerTests()
        {
            _repositoryMock = new Mock<IUserSubscriptionRepository>();
            _handler = new ExtendSubscriptionCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_WhenExtraDaysIsZeroOrLess_ShouldThrowArgumentException()
        {
            // Arrange
            var command = new ExtendSubscriptionCommand(Guid.NewGuid(), 0);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                     .WithMessage("تعداد روزهای تمدید باید بیشتر از صفر باشد.");
        }

        [Fact]
        public async Task Handle_WhenSubscriptionNotFound_ShouldThrowArgumentException()
        {
            // Arrange
            var command = new ExtendSubscriptionCommand(Guid.NewGuid(), 10);

            _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync((UserSubscription)null!);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                     .WithMessage("اشتراک مورد نظر یافت نشد.");
        }

        [Fact]
        public async Task Handle_WhenValid_ShouldExtendAndSaveToDatabase()
        {
            // Arrange
            var subscription = UserSubscription.Create(Guid.NewGuid(), Guid.NewGuid(), 10, 30, 1000);
            var command = new ExtendSubscriptionCommand(subscription.Id, 15);

            _repositoryMock.Setup(r => r.GetByIdAsync(command.SubscriptionId, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(subscription);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            _repositoryMock.Verify(r => r.UpdateAsync(subscription, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
