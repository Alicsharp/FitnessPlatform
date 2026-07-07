namespace FitnessPlatform.Application.Events
{
    public record WorkoutSessionCompletedEvent(
    Guid SessionId,
    string Title,
    int BurnedCalories,
    DateTime CompletedAt
);
}
