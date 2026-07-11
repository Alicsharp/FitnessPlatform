using System;
using FluentAssertions;
using Xunit;
using FitnessPlatform.Domain.Entities;

namespace FitnessPlatform.UnitTests.Domain.Entities
{
    public class TrainerTests
    {
        [Fact]
        public void CreateProfile_ShouldInitializeTrainerCorrectly()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            var trainer = Trainer.CreateProfile(userId, "استاد احمدی", "کراس‌فیت", 5); //[cite: 15]

            // Assert
            trainer.UserId.Should().Be(userId);
            trainer.FullName.Should().Be("استاد احمدی");
            trainer.Specialty.Should().Be("کراس‌فیت");
            trainer.ExperienceYears.Should().Be(5);
        }
    }
}