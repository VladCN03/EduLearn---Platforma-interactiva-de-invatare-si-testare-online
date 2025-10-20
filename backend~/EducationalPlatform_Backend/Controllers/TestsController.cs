using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EducationalPlatform_Backend.Data;
using EducationalPlatform_Backend.Models;

namespace EducationalPlatform_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Tests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Test>>> GetTests()
        {
            return await _context.Tests.ToListAsync();
        }

        [HttpGet("comments/byUser/{userId}")]
        public async Task<IActionResult> GetTestCommentsByUser(int userId)
        {
            var testComments = await _context.TestResults
                .Where(tr => tr.UserID == userId)
                .Include(tr => tr.Test)
                .Select(tr => new
                {
                    TestTitle = tr.Test.Title,
                    Comment = tr.Test.Comments,
                    SubmittedAt = tr.CompletedAt
                })
                .Where(x => x.Comment != null && x.Comment != "")
                .ToListAsync();

            return Ok(testComments);
        }


        // GET: api/Tests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Test>> GetTest(int id)
        {
            var test = await _context.Tests.FindAsync(id);

            if (test == null)
                return NotFound();

            return test;
        }

        // POST: api/Tests
        [HttpPost]
        public async Task<ActionResult<Test>> PostTest(Test test)
        {
            _context.Tests.Add(test);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTest), new { id = test.TestID }, test);
        }

        private bool TestExists(int id)
        {
            return _context.Tests.Any(e => e.TestID == id);
        }


        // PUT: api/Tests/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTest(int id, Test test)
        {
            if (id != test.TestID)
                return BadRequest();

            _context.Entry(test).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/Tests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTest(int id)
        {
            var test = await _context.Tests.FindAsync(id);
            if (test == null)
                return NotFound();

            _context.Tests.Remove(test);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
