using FitnessPlatform.Application.Features.Booking.Commands.CreateGroupClass;
using FitnessPlatform.Domain.Booking;
using FitnessPlatform.Domain.Repositories;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessPlatform.UnitTests.Application.Booking
{
    public class CreateGroupClassCommandHandlerTests
    {
        private readonly Mock<IGroupClassRepository> _repositoryMock;
        private readonly CreateGroupClassCommandHandler _handler;

        public CreateGroupClassCommandHandlerTests()
        {
            _repositoryMock = new Mock<IGroupClassRepository>();
            _handler = new CreateGroupClassCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldCreateClassAndSave()
        {
            // Arrange
            var command = new CreateGroupClassCommand("بادی پامپ", DateTime.UtcNow.AddDays(1), 15);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeEmpty();
            _repositoryMock.Verify(r => r.AddAsync(
                It.Is<GroupClass>(g =>
                    g.Title == "بادی پامپ" &&
                    g.MaxCapacity == 15),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
