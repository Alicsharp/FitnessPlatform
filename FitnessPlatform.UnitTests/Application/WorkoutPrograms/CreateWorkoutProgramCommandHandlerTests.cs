using FitnessPlatform.Application.Features.WorkoutPrograms.Commands.Create;
using FitnessPlatform.Domain.Entities;
using FitnessPlatform.Domain.Repositories;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;

namespace FitnessPlatform.UnitTests.Application.WorkoutPrograms
{
    public class CreateWorkoutProgramCommandHandlerTests
    {
        private readonly Mock<IWorkoutProgramRepository> _repositoryMock;
        private readonly CreateWorkoutProgramCommandHandler _handler;

        public CreateWorkoutProgramCommandHandlerTests()
        {
            _repositoryMock = new Mock<IWorkoutProgramRepository>();
            _handler = new CreateWorkoutProgramCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldCreateProgramAndSave()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new CreateWorkoutProgramCommand(
                userId,
                "برنامه فول بادی",
                "عضله‌سازی",
                DateTime.UtcNow,
                DateTime.UtcNow.AddDays(30));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeEmpty(); // شناسه باید با موفقیت تولید شود

            _repositoryMock.Verify(r => r.AddAsync(
                It.Is<WorkoutProgram>(p =>
                    p.Title == "برنامه فول بادی" &&
                    p.Objective == "عضله‌سازی" &&
                    p.UserId == userId),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
