using System;
using FluentAssertions;
using Xunit;
using FitnessPlatform.Domain.Entities;
using FitnessPlatform.Domain.ValueObjects; // فضای نام Email شما
using FitnessPlatform.Domain.Enums; // فضای نام Role شما

namespace FitnessPlatform.UnitTests.Domain.Entities
{
    public class UserTests
    {
        [Fact]
        public void Register_ShouldCreateUserWithCorrectData()
        {
            // Arrange
            Email email = null!; // ⚡️ تعریف دقیق نوع به جای var
            Role role = default; // ⚡️ تعریف دقیق نوع به جای var
            var passwordHash = "hashed_password_123";

            // Act
            var user = User.Register(email, passwordHash, role);

            // Assert
            user.PasswordHash.Should().Be(passwordHash);
            user.Role.Should().Be(role);
        }

        [Fact]
        public void ChangePassword_ShouldUpdatePasswordHash()
        {
            // Arrange
            Email email = null!;
            Role role = default;
            var user = User.Register(email, "old_hash", role);

            // Act
            user.ChangePassword("new_hash_456");

            // Assert
            user.PasswordHash.Should().Be("new_hash_456");
        }
    }
}