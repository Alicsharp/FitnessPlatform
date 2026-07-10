using FitnessPlatform.Domain.Enums;

namespace FitnessPlatform.Api.Contracts.Auth
{
    public record RegisterRequest(string Email, string Password, Role Role);
}
