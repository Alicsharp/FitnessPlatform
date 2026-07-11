using FitnessPlatform.Domain.Billing;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessPlatform.UnitTests.Domain.Billing
{
    public class SubscriptionPlanTests
    {
        [Fact]
        public void Create_WithValidData_ShouldCreatePlanAndSetIsActiveToTrue()
        {
            // Act
            var plan = SubscriptionPlan.Create("طرح طلایی", 500000, 12, 30);

            // Assert
            plan.Title.Should().Be("طرح طلایی");
            plan.Price.Should().Be(500000);
            plan.IsActive.Should().BeTrue();
            plan.DomainEvents.Should().NotBeEmpty(); // بررسی ثبت رویداد دامین
        }

        [Theory]
        [InlineData(-1000, 12, 30, "قیمت نمی‌تواند منفی باشد.")]
        [InlineData(500000, 0, 30, "تعداد جلسات باید بیشتر از صفر باشد.")]
        [InlineData(500000, 12, 0, "مدت اعتبار باید بیشتر از صفر روز باشد.")]
        public void Create_WithInvalidData_ShouldThrowArgumentException(
            decimal price, int sessions, int validityDays, string expectedMessage)
        {
            // Act
            Action act = () => SubscriptionPlan.Create("تست", price, sessions, validityDays);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage(expectedMessage);
        }

        [Fact]
        public void Deactivate_WhenPlanIsActive_ShouldSetIsActiveToFalse()
        {
            // Arrange
            var plan = SubscriptionPlan.Create("طرح نقره‌ای", 300000, 10, 30);

            // Act
            plan.Deactivate();

            // Assert
            plan.IsActive.Should().BeFalse();
        }

        [Fact]
        public void Deactivate_WhenPlanIsAlreadyInactive_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var plan = SubscriptionPlan.Create("طرح برنزی", 150000, 5, 15);
            plan.Deactivate(); // غیرفعال کردن اولیه

            // Act
            Action act = () => plan.Deactivate();

            // Assert
            act.Should().Throw<InvalidOperationException>()
               .WithMessage("این طرح قبلاً غیرفعال شده است.");
        }
    }
}
