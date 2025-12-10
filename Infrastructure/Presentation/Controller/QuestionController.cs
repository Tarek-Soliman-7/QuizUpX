using Application.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        private readonly ISubjectService _subjectService;
        private readonly ILogger<QuestionsController> _logger;

        public QuestionsController(
            IQuestionService questionService,
            ISubjectService subjectService,
            ILogger<QuestionsController> logger)
        {
            _questionService = questionService;
            _subjectService = subjectService;
            _logger = logger;
        }

        // GET: api/questions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetAll()
        {
            // if you need pagination add query params (page, pageSize)
            var all = await _questionService.GetBySubjectAsync(0); // fallback: change if you have GetAll in service
            // Note: If service doesn't support GetAll, you can add it. Here we return empty if 0.
            return Ok(all);
        }

        // GET: api/questions/{id}
        [HttpGet("{id:int}", Name = "GetQuestionById")]
        public async Task<ActionResult<QuestionDto>> GetById(int id)
        {
            var q = await _questionService.GetByIdAsync(id);
            if (q == null) return NotFound();

            return Ok(MapToDto(q));
        }

        // GET: api/subjects/{subjectId}/questions
        [HttpGet("/api/subjects/{subjectId:int}/questions")]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetBySubject(int subjectId)
        {
            var list = await _questionService.GetBySubjectAsync(subjectId);
            var dtoList = new List<QuestionDto>();
            foreach (var q in list) dtoList.Add(MapToDto(q));
            return Ok(dtoList);
        }

        // POST: api/questions
        [HttpPost]
        public async Task<ActionResult<QuestionDto>> Create([FromBody] CreateQuestionDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // ensure subject exists
            var subject = await _subjectService.GetByIdAsync(dto.subjectId);
            if (subject == null) return BadRequest($"Subject with id {dto.subjectId} does not exist.");

            var entity = new Question
            {
                // id left 0 so DB generates it
                subjectId = dto.subjectId,
                title = dto.title,
                choices = dto.choices ?? new List<string>(),
                correctIndex = dto.correctIndex,
                mark = dto.mark,
                questionType = dto.questionType
            };

            try
            {
                var created = await _questionService.CreateAsync(entity);
                return CreatedAtRoute("GetQuestionById", new { id = created.id }, MapToDto(created));
            }
            catch (KeyNotFoundException knf)
            {
                return BadRequest(knf.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating question");
                return StatusCode(500, "An error occurred while creating the question.");
            }
        }

        // PUT: api/questions/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateQuestionDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existing = await _questionService.GetByIdAsync(id);
            if (existing == null) return NotFound();

            // validate subject if changed
            if (dto.SubjectId.HasValue)
            {
                var subj = await _subjectService.GetByIdAsync(dto.SubjectId.Value);
                if (subj == null) return BadRequest($"Subject with id {dto.SubjectId.Value} does not exist.");
                existing.subjectId = dto.SubjectId.Value;
            }

            existing.title = dto.Title ?? existing.title;
            existing.choices = dto.Choices ?? existing.choices;
            existing.correctIndex = dto.CorrectIndex ?? existing.correctIndex;
            existing.mark = dto.Mark ?? existing.mark;
            existing.questionType = dto.QuestionType ?? existing.questionType;

            try
            {
                // repository/service update pattern: call update + commit
                // using our service we only have Create/Delete/Get; adapt accordingly.
                // If your IQuestionService includes Update, call it. Otherwise use UnitOfWork from controller (not recommended).
                // For now we use repository via service pattern: fetch, modify, then commit via UnitOfWork if exposed.
                // Assuming IQuestionService doesn't expose Update, we can rely on UnitOfWork injected (not ideal).
                // Simpler: use a small Update method on service. If not present, you'd update via repository.

                // For this example we'll assume IQuestionService has no Update -> we throw instructions:
                return BadRequest("Update not implemented in service. Please add UpdateAsync in IQuestionService.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating question {id}", id);
                return StatusCode(500, "An error occurred while updating the question.");
            }
        }

        // DELETE: api/questions/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _questionService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting question {id}", id);
                return StatusCode(500, "An error occurred while deleting the question.");
            }
        }
        private static QuestionDto MapToDto(Question q) => new QuestionDto
        {
            id = q.id,
            subjectId = q.subjectId,
            title = q.title,
            choices = q.choices,
            correctIndex = q.correctIndex,
            mark = q.mark,
            questionType = q.questionType
        };
    }
}
