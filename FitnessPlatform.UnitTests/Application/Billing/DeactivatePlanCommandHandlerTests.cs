using FitnessPlatform.Application.Features.Billing.Commands.DeactivatePlan;
using FitnessPlatform.Domain.Billing;
using FitnessPlatform.Domain.Repositories;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessPlatform.UnitTests.Application.Billing
{
    public class DeactivatePlanCommandHandlerTests
    {
        private readonly Mock<ISubscriptionPlanRepository> _repositoryMock;
        private readonly DeactivatePlanCommandHandler _handler;

        public DeactivatePlanCommandHandlerTests()
        {
            _repositoryMock = new Mock<ISubscriptionPlanRepository>();
            _handler = new DeactivatePlanCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_WhenPlanDoesNotExist_ShouldThrowArgumentException()
        {
            // Arrange
            var command = new DeactivatePlanCommand(Guid.NewGuid());

            _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync((SubscriptionPlan)null!);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                     .WithMessage("طرح مورد نظر یافت نشد.");
        }

        [Fact]
        public async Task Handle_WhenPlanExists_ShouldDeactivateAndUpdateDatabase()
        {
            // Arrange
            var command = new DeactivatePlanCommand(Guid.NewGuid());
            var plan = SubscriptionPlan.Create("تست", 1000, 10, 30);

            _repositoryMock.Setup(r => r.GetByIdAsync(command.PlanId, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(plan);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            plan.IsActive.Should().BeFalse();

            _repositoryMock.Verify(r => r.UpdateAsync(plan, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
