using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EducationalPlatform_Backend.Data;
using EducationalPlatform_Backend.Models;

namespace EducationalPlatform_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestResultsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TestResultsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/TestResults
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TestResult>>> GetTestResults()
        {
            return await _context.TestResults
                .Include(tr => tr.User)
                .Include(tr => tr.Test)
                .ToListAsync();
        }

        [HttpGet("byUser/{userId}")]
        public async Task<IActionResult> GetTestResultsByUser(int userId)
        {
            var results = await _context.TestResults
                .Where(r => r.UserID == userId)
                .Include(r => r.Test)
                .Select(r => new
                {
                    TestID = r.TestID,
                    TestTitle = r.Test.Title,
                    Score = r.Score,
                    SubmittedAt = r.CompletedAt
                })
                .ToListAsync();

            return Ok(results);
        }

        [HttpGet("maxGrade/{testID}")]
        public async Task<IActionResult> GetMaxGrade(int testID)
        {
            var maxGrade = await _context.TestResults
                .Where(tr => tr.TestID == testID)
                .OrderByDescending(tr => tr.CompletedAt)
                .Select(tr => tr.MaxGrade)
                .FirstOrDefaultAsync();

            return Ok(maxGrade);
        }


        // GET: api/TestResults/user/3/test/2
        [HttpGet("user/{userId}/test/{testId}")]
        public async Task<ActionResult<TestResult>> GetTestResult(int userId, int testId)
        {
            var result = await _context.TestResults
                .Include(tr => tr.User)
                .Include(tr => tr.Test)
                .FirstOrDefaultAsync(tr => tr.UserID == userId && tr.TestID == testId);

            if (result == null)
                return NotFound();

            return result;
        }

        // POST: api/TestResults
        [HttpPost]
        public async Task<ActionResult<TestResult>> PostTestResult(TestResult testResult)
        {
            _context.TestResults.Add(testResult);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTestResult), new { userId = testResult.UserID, testId = testResult.TestID }, testResult);
        }

        // DELETE: api/TestResults/user/3/test/2
        [HttpDelete("user/{userId}/test/{testId}")]
        public async Task<IActionResult> DeleteTestResult(int userId, int testId)
        {
            var result = await _context.TestResults.FindAsync(userId, testId);
            if (result == null)
                return NotFound();

            _context.TestResults.Remove(result);
            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}
