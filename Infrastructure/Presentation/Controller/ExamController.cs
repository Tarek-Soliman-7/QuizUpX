using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Abstraction.Contracts; // IExamService, IStudentService
using Shared.Dtos; // StartQuizRequest, StartQuizResponse, SubmitAttemptRequest, SubmitAttemptResponse, QuestionDto
// adjust namespaces above to match your project

namespace Presentation.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamController : ControllerBase
    {
        private readonly IExamService _examService;
        private readonly IStudentService _studentService;
        private readonly ILogger<ExamController> _logger;

        public ExamController(IExamService examService, IStudentService studentService, ILogger<ExamController> logger)
        {
            _examService = examService ?? throw new ArgumentNullException(nameof(examService));
            _studentService = studentService ?? throw new ArgumentNullException(nameof(studentService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Start an exam attempt for a subject. Verifies universityCode + pin (if provided).
        /// Returns attemptId and shuffled questions (no correct answers included).
        /// </summary>
        [HttpPost("start")]
        public async Task<IActionResult> Start([FromBody] StartQuizRequest req)
        {
            if (req == null) return BadRequest("Request body required.");
            if (string.IsNullOrWhiteSpace(req.UniversityCode)) return BadRequest("UniversityCode is required.");
            if (req.SubjectId <= 0) return BadRequest("SubjectId is required.");

            try
            {
                // Verify PIN (student credential). If pin is required by your policy, VerifyPinAsync should handle lockout.
                if (!string.IsNullOrWhiteSpace(req.Pin))
                {
                    var ok = await _studentService.VerifyPinAsync(req.UniversityCode.Trim(), req.Pin.Trim());
                    if (!ok)
                    {
                        // StudentService may also expose IsLockedOut, but VerifyPinAsync false is generic failure
                        var locked = await _studentService.IsLockedOutAsync(req.UniversityCode.Trim());
                        if (locked) return Forbid("Account locked due to multiple failed attempts.");
                        return Unauthorized();
                    }
                }
                else
                {
                    // If your policy requires PIN always, uncomment:
                    // return BadRequest(new { message = "PIN is required to start this exam." });
                }

                var result = await _examService.StartExamAsync(req);
                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Start exam validation failed.");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in Start.");
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        /// <summary>
        /// Submit student's answers for an attempt. Returns graded result and details.
        /// </summary>
        [HttpPost("submit")]
        public async Task<IActionResult> Submit([FromBody] SubmitResultDto req)
        {
            if (req == null) return BadRequest("Request body required.");
            if (req.AttemptId <= 0) return BadRequest("AttemptId is required.");

            try
            {
                var res = await _examService.SubmitAttemptAsync(req);
                return Ok(res);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Attempt not found." });
            }
            catch (InvalidOperationException ex)
            {
                // e.g. attempt already submitted
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in Submit.");
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

    }
}
