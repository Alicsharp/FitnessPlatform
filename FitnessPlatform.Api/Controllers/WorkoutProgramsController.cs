using FitnessPlatform.Application.Features.WorkoutPrograms.Commands.Create;
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
}
