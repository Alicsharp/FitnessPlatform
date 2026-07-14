using FitnessPlatform.Domain.Enums;
using FitnessPlatform.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessPlatform.Application.Features.WorkoutPrograms.Commands.Create
{
    public record CreateWorkoutProgramCommand(
     Guid UserId,
     string Title,
     string Objective,
     DateTime StartDate,
     DateTime EndDate,
     int AvailableMinutesPerDay,     
     EquipmentType Equipment      
     ) : IRequest<Guid>;
}
