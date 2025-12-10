using Application.Contracts;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
namespace Presentation.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubjectsController : ControllerBase
    {
        private readonly ISubjectService _subjectService;
        private readonly ILogger<SubjectsController> _logger;

        public SubjectsController(ISubjectService subjectService, ILogger<SubjectsController> logger)
        {
            _subjectService = subjectService;
            _logger = logger;
        }

        // GET: api/subjects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubjectDto>>> GetAll()
        {
            var subjects = await _subjectService.GetAllAsync();
            var result = new List<SubjectDto>();
            foreach (var s in subjects)
                result.Add(new SubjectDto { id = s.id, name = s.name, description = s.description });

            return Ok(result);
        }

        // GET: api/subjects/{id}
        [HttpGet("{id:int}", Name = "GetSubjectById")]
        public async Task<ActionResult<SubjectDto>> GetById(int id)
        {
            var s = await _subjectService.GetByIdAsync(id);
            if (s == null) return NotFound();
            return Ok(new SubjectDto { id = s.id, name = s.name, description = s.description });
        }

        // POST: api/subjects
        [HttpPost]
        public async Task<ActionResult<SubjectDto>> Create([FromBody] CreateSubjectDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var subject = new Subject { name = dto.name ,description=dto.description};
                var created = await _subjectService.CreateAsync(subject);

                var result = new SubjectDto { id = created.id, name = created.name, description = dto.description };
                return CreatedAtRoute("GetSubjectById", new { id = result.id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating subject");
                return StatusCode(500, "An error occurred while creating the subject.");
            }
        }
    }
}
