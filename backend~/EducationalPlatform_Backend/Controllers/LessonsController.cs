using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EducationalPlatform_Backend.Data;
using EducationalPlatform_Backend.Models;
using Microsoft.AspNetCore.Authorization;

namespace EducationalPlatform_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LessonsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Lessons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lesson>>> GetLessons()
        {
            return await _context.Lessons.ToListAsync();
        }

        // GET: api/Lessons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Lesson>> GetLesson(int id)
        {
            var lesson = await _context.Lessons.FindAsync(id);

            if (lesson == null)
                return NotFound();

            return lesson;
        }

        // POST: api/Lessons
        [Authorize(Roles = "Profesor")]
        [HttpPost]
        public async Task<IActionResult> PostLesson([FromForm] LessonUploadDto dto)
        {
            string? fileName = null;

            if (dto.File != null && dto.File.Length > 0)
            {
                fileName = Path.GetFileName(dto.File.FileName);
                var filePath = Path.Combine("wwwroot/fisiere", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.File.CopyToAsync(stream);
                }
            }

            var lesson = new Lesson
            {
                Title = dto.Title,
                Description = dto.Description,
                Author = dto.Author,
                Date = DateTime.Parse(dto.Date),
                Type = dto.Type,
                Attachment = fileName
            };

            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();

            Console.WriteLine($"Primit fisier: {dto.File?.FileName}, lungime: {dto.File?.Length}");

            return Ok(lesson);
        }

        // PUT: api/Lessons/5
        [Authorize(Roles = "Profesor")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLesson(int id, [FromForm] LessonUploadDto dto)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null) return NotFound();

            lesson.Title = dto.Title;
            lesson.Description = dto.Description;
            lesson.Author = dto.Author;
            lesson.Date = DateTime.Parse(dto.Date);
            lesson.Type = dto.Type;

            if (dto.File != null && dto.File.Length > 0)
            {
                var fileName = Path.GetFileName(dto.File.FileName);
                var filePath = Path.Combine("wwwroot/fisiere", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.File.CopyToAsync(stream);
                }

                lesson.Attachment = fileName;
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Lessons/5
        [Authorize(Roles = "Profesor")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null)
                return NotFound();

            _context.Lessons.Remove(lesson);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
