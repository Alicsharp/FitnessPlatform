using FitnessPlatform.Application.Exceptions;
using FitnessPlatform.Application.Features.Booking.Commands.Enroll;
using FitnessPlatform.Domain.Booking;
using FitnessPlatform.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace FitnessPlatform.UnitTests.Application.Booking
{
    public class EnrollInGroupClassCommandHandlerTests
    {
        private readonly Mock<IGroupClassRepository> _repositoryMock;
        private readonly EnrollInGroupClassCommandHandler _handler;

        public EnrollInGroupClassCommandHandlerTests()
        {
            _repositoryMock = new Mock<IGroupClassRepository>();
            _handler = new EnrollInGroupClassCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_WhenGroupClassNotFound_ShouldThrowArgumentException()
        {
            // Arrange
            var command = new EnrollInGroupClassCommand(Guid.NewGuid(), Guid.NewGuid());
            _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync((GroupClass)null!);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                     .WithMessage("کلاس مورد نظر یافت نشد.");
        }

        [Fact]
        public async Task Handle_WhenConcurrencyExceptionOccurs_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var command = new EnrollInGroupClassCommand(Guid.NewGuid(), Guid.NewGuid());
            var groupClass = GroupClass.Create("کراس فیت", DateTime.UtcNow.AddDays(1), 10);

            _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(groupClass);

            // ⚡️ در اینجا به ریپازیتوری قلابی می‌گوییم: وقتی خواستی آپدیت کنی، خطای همزمانی پرتاب کن
            _repositoryMock.Setup(r => r.UpdateAsync(groupClass, It.IsAny<CancellationToken>()))
                           .ThrowsAsync(new ConcurrencyException());

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                     .WithMessage("متأسفانه شخص دیگری در همین لحظه آخرین ظرفیت کلاس را رزرو کرد. لطفاً کلاس دیگری را انتخاب کنید.");
        }

        [Fact]
        public async Task Handle_WhenValid_ShouldEnrollMemberAndUpdateDatabase()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new EnrollInGroupClassCommand(userId, Guid.NewGuid());
            var groupClass = GroupClass.Create("یوگا", DateTime.UtcNow.AddDays(1), 10);

            _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(groupClass);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            groupClass.EnrolledMembers.Should().Contain(userId); // بررسی ثبت‌نام کاربر
            _repositoryMock.Verify(r => r.UpdateAsync(groupClass, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
