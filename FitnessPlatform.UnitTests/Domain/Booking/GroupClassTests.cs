using System;
using FluentAssertions;
using Xunit;
using FitnessPlatform.Domain.Booking;

namespace FitnessPlatform.UnitTests.Domain.Booking
{
    public class GroupClassTests
    {
        // ⚡️ تست اول: مسیر طلایی (Happy Path)
        [Fact]
        public void EnrollMember_WhenCapacityIsNotFull_ShouldAddMemberAndChangeVersion()
        {
            // Arrange
            var groupClass = GroupClass.Create("کراس فیت", DateTime.UtcNow.AddDays(1), 10);
            var memberId = Guid.NewGuid();
            var initialVersion = groupClass.Version;

            // Act
            groupClass.EnrollMember(memberId);

            // Assert
            groupClass.EnrolledMembers.Should().Contain(memberId);
            groupClass.EnrolledMembers.Should().HaveCount(1);
            groupClass.Version.Should().NotBe(initialVersion); // بررسی تغییر ورژن برای سیستم همزمانی
        }

        // ⚡️ تست دوم: بررسی قانون بیزینسی اول (جلوگیری از ثبت‌نام تکراری)
        [Fact]
        public void EnrollMember_WhenUserIsAlreadyEnrolled_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var groupClass = GroupClass.Create("یوگا", DateTime.UtcNow.AddDays(1), 10);
            var memberId = Guid.NewGuid();

            groupClass.EnrollMember(memberId); // ثبت‌نام بار اول با موفقیت انجام می‌شود

            // Act
            // وقتی انتظار ارور داریم، متد را داخل یک Action می‌ریزیم تا XUnit بتواند ارور را شکار کند
            Action act = () => groupClass.EnrollMember(memberId);

            // Assert
            act.Should().Throw<InvalidOperationException>()
               .WithMessage("شما قبلاً در این کلاس ثبت‌نام کرده‌اید.");
        }

        // ⚡️ تست سوم: بررسی قانون بیزینسی دوم (جلوگیری از ثبت‌نام بیش از ظرفیت)
        [Fact]
        public void EnrollMember_WhenCapacityIsFull_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var groupClass = GroupClass.Create("بادی پامپ", DateTime.UtcNow.AddDays(1), 1); // ظرفیت فقط ۱ نفر
            var firstMemberId = Guid.NewGuid();
            var secondMemberId = Guid.NewGuid();

            groupClass.EnrollMember(firstMemberId); // کلاس پر شد

            // Act
            Action act = () => groupClass.EnrollMember(secondMemberId); // تلاش نفر دوم برای ثبت‌نام

            // Assert
            act.Should().Throw<InvalidOperationException>()
               .WithMessage("ظرفیت این کلاس تکمیل شده است.");
        }
    }
}