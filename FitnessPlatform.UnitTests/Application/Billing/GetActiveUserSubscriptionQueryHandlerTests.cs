using FitnessPlatform.Application.Features.Billing.Queries.GetActiveUserSubscription;
using FitnessPlatform.Domain.Billing;
using FitnessPlatform.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace FitnessPlatform.UnitTests.Application.Billing
{
    public class GetActiveUserSubscriptionQueryHandlerTests
    {
        private readonly Mock<IUserSubscriptionRepository> _repositoryMock;
        private readonly GetActiveUserSubscriptionQueryHandler _handler;

        public GetActiveUserSubscriptionQueryHandlerTests()
        {
            _repositoryMock = new Mock<IUserSubscriptionRepository>();
            _handler = new GetActiveUserSubscriptionQueryHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_WhenUserHasNoActiveSubscription_ShouldReturnNull()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var query = new GetActiveUserSubscriptionQuery(userId);

            _repositoryMock.Setup(r => r.GetActiveSubscriptionByUserIdAsync(userId, It.IsAny<CancellationToken>()))
                           .ReturnsAsync((UserSubscription)null!);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Handle_WhenUserHasActiveSubscription_ShouldReturnMappedDto()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var planId = Guid.NewGuid();
            var query = new GetActiveUserSubscriptionQuery(userId);

            var subscription = UserSubscription.Create(userId, planId, 12, 30, 500000);

            _repositoryMock.Setup(r => r.GetActiveSubscriptionByUserIdAsync(userId, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(subscription);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.PlanId.Should().Be(planId);
            result.TotalSessions.Should().Be(12);
            result.RemainingSessions.Should().Be(12);
            result.IsValid.Should().BeTrue();
            result.StartDate.Should().Be(subscription.StartDate);
        }
    }
}
