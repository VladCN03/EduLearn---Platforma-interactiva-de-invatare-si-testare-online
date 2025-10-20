using System.Collections.Generic;

namespace EducationalPlatform_Backend.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string? Avatar { get; set; }

        public ICollection<CompletedLesson> CompletedLessons { get; set; } = new List<CompletedLesson>();
        public ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();


        // For authentication
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
