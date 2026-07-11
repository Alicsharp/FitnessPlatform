using FitnessPlatform.Application.Features.Booking.Queries.GetAvailableClasses;
using FitnessPlatform.Domain.Booking;
using FitnessPlatform.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace FitnessPlatform.UnitTests.Application.Booking
{
    public class GetAvailableClassesQueryHandlerTests
    {
        private readonly Mock<IGroupClassRepository> _repositoryMock;
        private readonly GetAvailableClassesQueryHandler _handler;

        public GetAvailableClassesQueryHandlerTests()
        {
            _repositoryMock = new Mock<IGroupClassRepository>();
            _handler = new GetAvailableClassesQueryHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_WhenClassesExist_ShouldReturnMappedDtos()
        {
            // Arrange
            var query = new GetAvailableClassesQuery();

            var class1 = GroupClass.Create("کراس فیت", DateTime.UtcNow.AddDays(1), 10);
            class1.EnrollMember(Guid.NewGuid()); // اضافه کردن یک عضو برای چک کردن شمارنده کلاینت

            var class2 = GroupClass.Create("زومبا", DateTime.UtcNow.AddDays(2), 20);

            _repositoryMock.Setup(r => r.GetAvailableClassesAsync(It.IsAny<CancellationToken>()))
                           .ReturnsAsync(new List<GroupClass> { class1, class2 });

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            var resultList = result.ToList();
            resultList.Should().HaveCount(2);

            // بررسی عملیات Mapping و انتقال تعداد اعضا به DTO
            resultList[0].Title.Should().Be("کراس فیت");
            resultList[0].EnrolledCount.Should().Be(1);

            resultList[1].Title.Should().Be("زومبا");
            resultList[1].EnrolledCount.Should().Be(0);
        }
    }
}
