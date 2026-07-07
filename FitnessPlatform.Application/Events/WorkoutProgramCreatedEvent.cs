using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessPlatform.Application.Events
{
    public record WorkoutProgramCreatedEvent(Guid ProgramId, string Title, DateTime CreatedAt);
}
