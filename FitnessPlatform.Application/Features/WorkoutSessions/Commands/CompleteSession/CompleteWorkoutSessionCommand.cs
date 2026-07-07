using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessPlatform.Application.Features.WorkoutSessions.Commands.CompleteSession
{
    // این دستور می‌گوید: لطفاً جلسه تمرینی با این شناسه را به پایان برسان!
    // خروجی آن یک متغیر bool است (موفقیت یا شکست)
    public record CompleteWorkoutSessionCommand(Guid SessionId) : IRequest<bool>;
}
