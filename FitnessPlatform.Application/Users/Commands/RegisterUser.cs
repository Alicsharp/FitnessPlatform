using FitnessPlatform.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessPlatform.Application.Users.Commands
{
    // ورودی‌های خام از سمت کنترلر
    public record RegisterUserCommand(string Email, string Password, Role Role) : IRequest<Guid>;
}
