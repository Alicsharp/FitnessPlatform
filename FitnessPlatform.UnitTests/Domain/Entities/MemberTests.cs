using System;
using FluentAssertions;
using Xunit;
using FitnessPlatform.Domain.Entities;

namespace FitnessPlatform.UnitTests.Domain.Entities
{
    public class MemberTests
    {
        [Fact]
        public void CreateProfile_ShouldInitializeCorrectly()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            var member = Member.CreateProfile(userId, "علی محمدی", 80.5m, 185.0m);

            // Assert
            member.UserId.Should().Be(userId);
            member.FullName.Should().Be("علی محمدی");
            member.Weight.Should().Be(80.5m);
            member.Height.Should().Be(185.0m);
            member.RemainingSessions.Should().Be(0); //[cite: 14]
        }

        [Fact]
        public void PaySubscription_ShouldIncreaseSessionsAndSetEndDate()
        {
            // Arrange
            var member = Member.CreateProfile(Guid.NewGuid(), "تست", 80, 180);

            // Act
            member.PaySubscription(12, 30); //[cite: 14]

            // Assert
            member.RemainingSessions.Should().Be(12);
            member.SubscriptionEndDate.Should().BeAfter(DateTime.UtcNow);
        }

        [Fact]
        public void DeductSession_WhenValid_ShouldDecreaseRemainingSessions()
        {
            // Arrange
            var member = Member.CreateProfile(Guid.NewGuid(), "تست", 80, 180);
            member.PaySubscription(10, 30); // شارژ اولیه[cite: 14]

            // Act
            member.DeductSession(); //[cite: 14]

            // Assert
            member.RemainingSessions.Should().Be(9);
        }

        [Fact]
        public void DeductSession_WhenNoSessionsLeft_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var member = Member.CreateProfile(Guid.NewGuid(), "تست", 80, 180);
            // کاربری که شارژ نکرده است

            // Act
            Action act = () => member.DeductSession(); //[cite: 14]

            // Assert
            act.Should().Throw<InvalidOperationException>()
               .WithMessage("اشتراک شما به پایان رسیده است."); //[cite: 14]
        }
    }
}