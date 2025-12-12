using Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Abstraction.Contracts;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubmitController : ControllerBase
    {
        private readonly IExamService _examService;
        private readonly ILogger<SubmitController> _logger;

        public SubmitController(IExamService examService, ILogger<SubmitController> logger)
        {
            _examService = examService ?? throw new ArgumentNullException(nameof(examService));
            _logger = logger;
        }

        /// <summary>
        /// Student submits answers for a subject.
        /// Body: SubmitDto { subjectId, answers: { "<questionId>": value, ... } }
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Submit([FromBody] SubmitDto dto)
        {
            if (dto == null)
                return BadRequest("Payload is empty.");

            if (dto.answers == null || !dto.answers.Any())
                return BadRequest("Answers are required.");

            try
            {
                // optional: get user id if auth used
                var userId = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                // optional metadata
                var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
                var device = Request.Headers["User-Agent"].ToString();

                // call the exam service (grading). Assumes GradeAsync returns SubmitResultDto
                var result = await _examService.GradeAndSaveAttemptAsync(dto);

                // Optionally hide correct answers from the student immediately:
                // Uncomment the next lines to remove correctIndex from details returned to the caller.
                // foreach (var d in result.Details) d.CorrectIndex = -1;

                // You can also add attempt id or other metadata if the service saves attempts.

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid submit for subject {SubjectId}", dto.subjectId);
                return BadRequest(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Bad request on submit");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing submit for subject {SubjectId}", dto.subjectId);
                return StatusCode(500, new { message = "An error occurred while processing the submit." });
            }
        }
    }
}
