using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Abstraction.Contracts;
using Shared.Dtos;

namespace Presentation.Controller
{
    [ApiController]
    [Route("api/questions")]
    public class QuestionController : ControllerBase
    {
        private readonly IQuizService _quizService;

        public QuestionController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBySubject([FromQuery] int subjectId)
        {
            if (subjectId <= 0)
                return BadRequest("Invalid subject id");

            try
            {
                var questions = await _quizService.GetQuestionsAsync(subjectId);
                return Ok(questions);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to load questions");
            }
        }
    }

}
