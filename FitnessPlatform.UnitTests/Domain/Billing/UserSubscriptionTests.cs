using FitnessPlatform.Domain.Billing;
using FluentAssertions;

namespace FitnessPlatform.UnitTests.Domain.Billing
{
    public class UserSubscriptionTests
    {
        [Fact]
        public void Create_ShouldInitializeCorrectlyAndSetRemainingSessions()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var planId = Guid.NewGuid();

            // Act
            var subscription = UserSubscription.Create(userId, planId, 12, 30, 500000);

            // Assert
            subscription.TotalSessions.Should().Be(12);
            subscription.RemainingSessions.Should().Be(12);
            subscription.IsValid.Should().BeTrue();
            subscription.EndDate.Should().BeAfter(subscription.StartDate);
        }

        [Fact]
        public void UseSession_WhenValid_ShouldDecrementRemainingSessions()
        {
            // Arrange
            var subscription = UserSubscription.Create(Guid.NewGuid(), Guid.NewGuid(), 10, 30, 1000);
            var initialSessions = subscription.RemainingSessions;

            // Act
            subscription.UseSession();

            // Assert
            subscription.RemainingSessions.Should().Be(initialSessions - 1);
        }

        [Fact]
        public void UseSession_WhenNoSessionsRemaining_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var subscription = UserSubscription.Create(Guid.NewGuid(), Guid.NewGuid(), 1, 30, 1000);
            subscription.UseSession(); // مصرف تنها جلسه موجود

            // Act
            Action act = () => subscription.UseSession();

            // Assert
            act.Should().Throw<InvalidOperationException>()
               .WithMessage("اشتراک شما منقضی شده یا جلسات شما به پایان رسیده است. امکان ثبت تمرین وجود ندارد.");
        }

        [Fact]
        public void Expire_ShouldZeroRemainingSessionsAndSetEndDateToPast()
        {
            // Arrange
            var subscription = UserSubscription.Create(Guid.NewGuid(), Guid.NewGuid(), 10, 30, 1000);

            // Act
            subscription.Expire();

            // Assert
            subscription.RemainingSessions.Should().Be(0);
            subscription.EndDate.Should().BeBefore(DateTime.UtcNow);
            subscription.IsValid.Should().BeFalse();
        }

        [Fact]
        public void ExtendEndDate_ShouldAddDaysToExistingEndDate()
        {
            // Arrange
            var subscription = UserSubscription.Create(Guid.NewGuid(), Guid.NewGuid(), 10, 30, 1000);
            var originalEndDate = subscription.EndDate;
            int extraDays = 5;

            // Act
            subscription.ExtendEndDate(extraDays);

            // Assert
            subscription.EndDate.Should().BeCloseTo(originalEndDate.AddDays(extraDays), TimeSpan.FromSeconds(1));
        }
    }
}
