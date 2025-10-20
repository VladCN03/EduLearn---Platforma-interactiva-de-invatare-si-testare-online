namespace EducationalPlatform_Backend.Models
{
    public class LessonUploadDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string Date { get; set; }
        public string Type { get; set; } // curs sau lab
        public IFormFile? File { get; set; }
    }
}
