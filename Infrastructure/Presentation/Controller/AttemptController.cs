using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contracts;

namespace Presentation.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttemptController : ControllerBase
    {
        private readonly IAttemptService _attemptService;

        public AttemptController(IAttemptService attemptService) => _attemptService = attemptService;

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var details = await _attemptService.GetAttemptDetailsAsync(id);
            return Ok(details);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserAttempts(string userId, int skip = 0, int take = 50)
        {
            var list = await _attemptService.GetUserAttemptsAsync(userId, skip, take);
            return Ok(list);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _attemptService.DeleteAttemptAsync(id);
            return NoContent();
        }
    }
}
