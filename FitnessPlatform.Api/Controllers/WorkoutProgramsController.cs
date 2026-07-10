using FitnessPlatform.Application.Features.WorkoutPrograms.Commands.Create;
using FitnessPlatform.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FitnessPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class WorkoutProgramsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WorkoutProgramsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProgram([FromBody] CreateWorkoutProgramCommand command)
        {
            // کنترلر هیچ منطقی ندارد! فقط درخواست را به MediatR پاس می‌دهد
            var programId = await _mediator.Send(command);

            // برگرداندن کد 200 (OK) همراه با شناسه برنامه ساخته شده
            return Ok(new { Message = "برنامه تمرینی با موفقیت ساخته شد", ProgramId = programId });
        }
    }

    // ==========================================
    // DTOs (Data Transfer Objects)
    // ==========================================
    // این رکوردها فقط برای دریافت اطلاعات از بدنه درخواست (JSON) استفاده می‌شوند
    public record RegisterRequest(string Email, string Password, Role Role);

    public record LoginRequest(string Email, string Password);
}
 