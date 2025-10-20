using System;

namespace EducationalPlatform_Backend.Models
{
    public class TestResult
    {
        public int UserID { get; set; }
        public int TestID { get; set; }
        public int Score { get; set; }
        public int MaxGrade { get; set; }
        public DateTime CompletedAt { get; set; } = DateTime.Now;

        public User? User { get; set; }
        public Test? Test { get; set; }
    }
}
