using FitnessPlatform.Application.Features.Billing.Queries.GetActivePlans;
using FitnessPlatform.Domain.Billing;
using FitnessPlatform.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace FitnessPlatform.UnitTests.Application.Billing
{
    public class GetActivePlansQueryHandlerTests
    {
        private readonly Mock<ISubscriptionPlanRepository> _repositoryMock;
        private readonly GetActivePlansQueryHandler _handler;

        public GetActivePlansQueryHandlerTests()
        {
            _repositoryMock = new Mock<ISubscriptionPlanRepository>();
            _handler = new GetActivePlansQueryHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_WhenNoActivePlansExist_ShouldReturnEmptyList()
        {
            // Arrange
            var query = new GetActivePlansQuery();

            _repositoryMock.Setup(r => r.GetActivePlansAsync(It.IsAny<CancellationToken>()))
                           .ReturnsAsync(new List<SubscriptionPlan>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_WhenActivePlansExist_ShouldReturnMappedDtos()
        {
            // Arrange
            var query = new GetActivePlansQuery();

            var plan1 = SubscriptionPlan.Create("طرح ۱", 1000, 10, 30);
            var plan2 = SubscriptionPlan.Create("طرح ۲", 2000, 20, 60);
            var plansList = new List<SubscriptionPlan> { plan1, plan2 };

            _repositoryMock.Setup(r => r.GetActivePlansAsync(It.IsAny<CancellationToken>()))
                           .ReturnsAsync(plansList);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            var resultList = result.ToList();
            resultList.Should().HaveCount(2);

            // بررسی صحت عملیات Mapping
            resultList[0].Title.Should().Be("طرح ۱");
            resultList[0].Price.Should().Be(1000);
            resultList[1].Title.Should().Be("طرح ۲");
            resultList[1].ValidityDays.Should().Be(60);
        }
    }
}
