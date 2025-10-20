namespace EducationalPlatform_Backend.Models
{
    public class CompletedLessonDto
    {
        public int UserID { get; set; }
        public int LessonID { get; set; }
        public DateTime CompletedAt { get; set; }
    }
}
