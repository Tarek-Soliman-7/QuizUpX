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
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpPost("verify-pin")]
        public async Task<IActionResult> VerifyPin([FromBody] VerifyPinRequest req)
        {
            var ok = await _studentService.VerifyPinAsync(req.UniversityCode, req.Pin);

            if (!ok)
                return Unauthorized();

            return Ok(new { message = "PIN accepted" });
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateStudent([FromBody] CreateStudentDto dto)
        {
            try
            {
                var student = await _studentService.CreateStudentAsync(
                    dto.UniversityCode,
                    dto.FullName,
                    dto.Pin
                );

                return Ok(new
                {
                    message = "Student created successfully.",
                    studentId = student.Id
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
