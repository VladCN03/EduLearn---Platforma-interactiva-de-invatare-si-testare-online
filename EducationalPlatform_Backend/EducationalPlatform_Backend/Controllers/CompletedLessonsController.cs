using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EducationalPlatform_Backend.Data;
using EducationalPlatform_Backend.Models;

namespace EducationalPlatform_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompletedLessonsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CompletedLessonsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CompletedLessons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompletedLesson>>> GetCompletedLessons()
        {
            return await _context.CompletedLessons
                .Include(cl => cl.User)
                .Include(cl => cl.Lesson)
                .ToListAsync();
        }

        // GET: api/CompletedLessons/user/3/lesson/7
        [HttpGet("user/{userId}/lesson/{lessonId}")]
        public async Task<ActionResult<CompletedLesson>> GetCompletedLesson(int userId, int lessonId)
        {
            var result = await _context.CompletedLessons
                .Include(cl => cl.User)
                .Include(cl => cl.Lesson)
                .FirstOrDefaultAsync(cl => cl.UserID == userId && cl.LessonID == lessonId);

            if (result == null)
                return NotFound();

            return result;
        }

        [HttpGet("byUser/{userId}")]
        public async Task<IActionResult> GetCompletedLessonsByUser(int userId)
        {
            var lessons = await _context.CompletedLessons
                .Where(cl => cl.UserID == userId)
                .Include(cl => cl.Lesson)
                .Select(cl => new
                {
                    Title = cl.Lesson.Title,
                    Author = cl.Lesson.Author,
                    CompletedAt = cl.CompletedAt
                })
                .ToListAsync();

            return Ok(lessons);
        }


        // POST: api/CompletedLessons
        [HttpPost]
        public async Task<ActionResult<CompletedLessonDto>> PostCompletedLesson(CompletedLessonDto dto)
        {
            var completed = new CompletedLesson
            {
                UserID = dto.UserID,
                LessonID = dto.LessonID,
                CompletedAt = dto.CompletedAt
            };

            _context.CompletedLessons.Add(completed);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCompletedLesson),
                new { userId = completed.UserID, lessonId = completed.LessonID },
                dto); // returnezi doar DTO
        }

        // DELETE: api/CompletedLessons/user/3/lesson/7
        [HttpDelete("user/{userId}/lesson/{lessonId}")]
        public async Task<IActionResult> DeleteCompletedLesson(int userId, int lessonId)
        {
            var completedLesson = await _context.CompletedLessons.FindAsync(userId, lessonId);
            if (completedLesson == null)
                return NotFound();

            _context.CompletedLessons.Remove(completedLesson);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
