using System;
using System.Collections.Generic;

namespace EducationalPlatform_Backend.Models
{
    public class Lesson
    {
        public int LessonID { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string Author { get; set; } = null!;
        public DateTime Date { get; set; }
        public string? Attachment { get; set; }
        public string Type { get; set; } = null!; // "curs" sau "lab"

        public ICollection<CompletedLesson>? CompletedLessons { get; set; }
    }
}
