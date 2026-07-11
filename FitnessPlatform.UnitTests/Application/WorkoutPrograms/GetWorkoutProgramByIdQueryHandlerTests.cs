using FitnessPlatform.Application.Features.WorkoutPrograms.Queries.GetProgram;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using System.Text;

namespace FitnessPlatform.UnitTests.Application.WorkoutPrograms
{
    public class GetWorkoutProgramByIdQueryHandlerTests
    {
        private readonly Mock<IDistributedCache> _cacheMock;
        private readonly GetWorkoutProgramByIdQueryHandler _handler;

        public GetWorkoutProgramByIdQueryHandlerTests()
        {
            _cacheMock = new Mock<IDistributedCache>();
            _handler = new GetWorkoutProgramByIdQueryHandler(_cacheMock.Object);
        }

        [Fact]
        public async Task Handle_WhenDataExistsInCache_ShouldReturnDataFromRedis()
        {
            // Arrange
            var query = new GetWorkoutProgramByIdQuery(Guid.NewGuid());
            string cacheKey = $"WorkoutProgram_{query.Id}";

            // دیتای فیک که به صورت آرایه بایتی درآورده شده تا متد اصلی کش شبیه‌سازی شود
            string fakeCachedData = "{\"Title\":\"برنامه کش شده\"}";
            byte[] fakeBytes = Encoding.UTF8.GetBytes(fakeCachedData);

            _cacheMock.Setup(c => c.GetAsync(cacheKey, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(fakeBytes);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().StartWith("[خوانده شده از Redis]");
            result.Should().Contain("برنامه کش شده");

            // اطمینان از اینکه اگر دیتا در کش بود، دیگر عملیات ذخیره‌سازی (Set) اتفاق نمی‌افتد
            _cacheMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenDataNotInCache_ShouldFetchFromDbAndSaveToCache()
        {
            // Arrange
            var query = new GetWorkoutProgramByIdQuery(Guid.NewGuid());
            string cacheKey = $"WorkoutProgram_{query.Id}";

            // شبیه‌سازی Cache Miss (خالی بودن ردیس)
            _cacheMock.Setup(c => c.GetAsync(cacheKey, It.IsAny<CancellationToken>()))
                      .ReturnsAsync((byte[])null!);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().StartWith("[خوانده شده از SQL Server و ذخیره در کش]");
            result.Should().Contain("برنامه فشرده کاهش وزن"); // تایتل فیک موجود در کد

            // ⚡️ اطمینان از اینکه بعد از خواندن از دیتابیس، دیتا حتماً در کش ذخیره شده باشد
            _cacheMock.Verify(c => c.SetAsync(
                cacheKey,
                It.IsAny<byte[]>(),
                It.IsAny<DistributedCacheEntryOptions>(),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
