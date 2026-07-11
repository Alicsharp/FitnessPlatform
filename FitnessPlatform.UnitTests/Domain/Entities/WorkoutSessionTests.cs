// using FitnessPlatform.Domain.ValueObjects; // فضای نام Email شما
// using FitnessPlatform.Domain.Enums; // فضای نام Role شما

using FitnessPlatform.Domain.Entities;
using FluentAssertions;

namespace FitnessPlatform.UnitTests.Domain.Entities
{
    public class WorkoutSessionTests
    {
        [Fact]
        public void MarkAsCompleted_WhenNotCompleted_ShouldSetIsCompletedToTrue()
        {
            // Arrange
            var session = WorkoutSession.Create("هوازی", DateTime.UtcNow, 500); //[cite: 18]

            // Act
            session.MarkAsCompleted(); //[cite: 18]

            // Assert
            session.IsCompleted.Should().BeTrue(); //[cite: 18]
        }

        [Fact]
        public void MarkAsCompleted_WhenAlreadyCompleted_ShouldThrowException()
        {
            // Arrange
            var session = WorkoutSession.Create("هوازی", DateTime.UtcNow, 500); //[cite: 18]
            session.MarkAsCompleted(); // بار اول با موفقیت تکمیل می‌شود //[cite: 18]

            // Act
            Action act = () => session.MarkAsCompleted(); //[cite: 18]

            // Assert
            act.Should().Throw<InvalidOperationException>()
               .WithMessage("این جلسه قبلاً تکمیل شده است."); //[cite: 18]
        }
    }
}