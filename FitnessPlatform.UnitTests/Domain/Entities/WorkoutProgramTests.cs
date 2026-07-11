// using FitnessPlatform.Domain.ValueObjects; // فضای نام Email شما
// using FitnessPlatform.Domain.Enums; // فضای نام Role شما

using FitnessPlatform.Domain.Entities;
using FluentAssertions;

namespace FitnessPlatform.UnitTests.Domain.Entities
{
    public class WorkoutProgramTests
    {
        [Fact]
        public void Create_ShouldInitializeProgramAndAddDomainEvent()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            // ⚡️ با پاس دادن null! به صورت مستقیم، نیازی به تعریف var duration نداریم
            var program = WorkoutProgram.Create(userId, "برنامه حجمی", "افزایش عضله", null!);

            // Assert
            program.Title.Should().Be("برنامه حجمی");
            program.IsActive.Should().BeTrue();
            program.DomainEvents.Should().NotBeEmpty();
        }

        [Fact]
        public void AddSession_WhenSessionIsDuplicate_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var program = WorkoutProgram.Create(userId, "برنامه حجمی", "افزایش عضله", null!);

            var date = DateTime.UtcNow;
            var session1 = WorkoutSession.Create("پرس سینه", date, 300);
            var session2 = WorkoutSession.Create("پرس سینه", date, 350);

            program.AddSession(session1);

            // Act
            Action act = () => program.AddSession(session2);

            // Assert
            act.Should().Throw<InvalidOperationException>()
               .WithMessage("شما قبلاً این تمرین را برای این تاریخ ثبت کرده‌اید.");
        }
    }
}