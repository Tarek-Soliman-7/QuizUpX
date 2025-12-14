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
    [Route("api/quizzes")]
    public class QuizController : ControllerBase
    {
        private readonly IQuizService _quizService;

        public QuizController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        [HttpPost("submit")]
        public async Task<IActionResult> Submit([FromBody] SubmitQuizDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid submission data");

            try
            {
                var result = await _quizService.SubmitQuizAsync(dto);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to submit quiz");
            }
        }

        // Review answers
        [HttpGet("attempt/{attemptId}/review")]
        public async Task<IActionResult> Review(int attemptId)
        {
            if (attemptId <= 0)
                return BadRequest("Invalid attempt id");

            try
            {
                var review = await _quizService.GetReviewAsync(attemptId);
                return Ok(review);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to load review");
            }
        }
        [HttpGet("subjects")]
        public async Task<IActionResult> Subjects()
        {
            var subjects = await _quizService.GetAllSubjectAsync();
            return Ok(subjects);
        }

    }

}
