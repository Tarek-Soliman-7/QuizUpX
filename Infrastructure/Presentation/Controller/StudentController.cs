using Microsoft.AspNetCore.Mvc;
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
    [Route("api/students")]
    public class StudentController : ControllerBase
    {
        private readonly IQuizService _quizService;

        public StudentController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] StudentLoginRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid request data");

            try
            {
                var result = await _quizService.LoginAsync(dto);

                if (!result.Success)
                    return Unauthorized();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred");
            }
        }
    }
}
