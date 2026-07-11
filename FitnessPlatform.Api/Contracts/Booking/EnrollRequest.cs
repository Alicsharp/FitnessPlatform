namespace FitnessPlatform.Api.Contracts.Booking
{
    // این رکورد فقط وظیفه دریافت دیتای JSON از کلاینت را بر عهده دارد
    public record EnrollRequest(Guid UserId);
}
