using FitnessPlatform.Application.Features.Billing.Commands.CreatePlan;
using FitnessPlatform.Domain.Billing;
using FitnessPlatform.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace FitnessPlatform.UnitTests.Application.Billing
{
    public class CreateSubscriptionPlanCommandHandlerTests
    {
        private readonly Mock<ISubscriptionPlanRepository> _repositoryMock;
        private readonly CreateSubscriptionPlanCommandHandler _handler;

        public CreateSubscriptionPlanCommandHandlerTests()
        {
            _repositoryMock = new Mock<ISubscriptionPlanRepository>();
            _handler = new CreateSubscriptionPlanCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldCreatePlanAndSaveToDatabase()
        {
            // Arrange
            var command = new CreateSubscriptionPlanCommand("طرح ۶ ماهه", 1500000, 72, 180);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeEmpty();

            _repositoryMock.Verify(r => r.AddAsync(
                It.Is<SubscriptionPlan>(p =>
                    p.Title == "طرح ۶ ماهه" &&
                    p.Price == 1500000 &&
                    p.TotalSessions == 72 &&
                    p.ValidityDays == 180),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
