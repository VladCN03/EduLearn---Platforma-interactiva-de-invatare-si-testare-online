using System;

namespace EducationalPlatform_Backend.Models
{
    public class CompletedLesson
    {
        public int UserID { get; set; }
        public int LessonID { get; set; }
        public DateTime CompletedAt { get; set; } = DateTime.Now;

        public User? User { get; set; }
        public Lesson? Lesson { get; set; }
    }
}
